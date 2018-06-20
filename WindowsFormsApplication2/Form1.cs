using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication2.MyFigures;


//HACK: растровая заливка
//HACK: круг - радиус
//HACK: квадрат - стороны
//HACK: несколько фигур на полотне
//HACK: регулировка толщины линий
//HACK: сохранять изображения
//HACK: масштабирование
//TODO: окошко для демонстрации работы
//UNDONE: срок -пятница
//TODO: количество точек за секунду
//HACK: качественнее круг
//HACK: побольше панель

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        private int SizeSideSquare { get; set; }
        public int RadiusOfCircle { get; private set; }
        public Color ColorPouring { get; private set; }

        Color CurrentColor = Color.Black;
        bool ifPressed = false; 
        bool pen = false;
        Point CurrentPoint;
        Point CurrentPoint_1;
        Point CurrentPoint_2;
        Point CurrentPoint_3;
        Point CurrentPoint_4;
        bool line = false;
        bool thr = false;
        bool kr = false;
        bool kvad = false;
        Point PrevPoint;    
        int counter = 0;
        int x, y, l, m;
        Graphics g;       
        Pen currentPen;

        Queue<Point> calculatedPoints = new Queue<Point>();
        CalcPointsFigureAsync calcPointsAsync = new CalcPointsFigureAsync();
        Task taskCalcPoints;

        private uint DotesPerSecond { get; set; }
        public uint DotesPerSec { get; private set; }
        public int MillisecOnDote { get; private set; }

        bool isPouring = false;

        private Bitmap PaintZone(Bitmap sourceImage, int x, int y, Color color, Color borderColor)
        {
            Bitmap image = (Bitmap)sourceImage.Clone();
            Stack<Point> points = new Stack<Point>();
            points.Push(new Point(x, y));

            Point currentPoint;
            while (points.Count != 0)
            {
                currentPoint = points.Pop();
                image.SetPixel(currentPoint.X, currentPoint.Y, color);

                if (currentPoint.X > 0 && currentPoint.X < image.Width && currentPoint.Y + 1 > 0 && currentPoint.Y + 1 < image.Height)
                {
                    Color topPixel = image.GetPixel(currentPoint.X, currentPoint.Y + 1);
                    if (topPixel.ToArgb() != borderColor.ToArgb() && topPixel.ToArgb() != color.ToArgb())
                    {
                        points.Push(new Point(currentPoint.X, currentPoint.Y + 1));
                    }
                }

                if (currentPoint.X + 1 > 0 && currentPoint.X + 1 < image.Width && currentPoint.Y > 0 && currentPoint.Y < image.Height)
                {
                    Color rightPixel = image.GetPixel(currentPoint.X + 1, currentPoint.Y);
                    if (rightPixel.ToArgb() != borderColor.ToArgb() && rightPixel.ToArgb() != color.ToArgb())
                    {
                        points.Push(new Point(currentPoint.X + 1, currentPoint.Y));
                    }
                }


                if (currentPoint.X > 0 && currentPoint.X < image.Width && currentPoint.Y - 1 > 0 && currentPoint.Y - 1 < image.Height)
                {
                    Color bottomPixel = image.GetPixel(currentPoint.X, currentPoint.Y - 1);
                    if (bottomPixel.ToArgb() != borderColor.ToArgb() && bottomPixel.ToArgb() != color.ToArgb())
                    {
                        points.Push(new Point(currentPoint.X, currentPoint.Y - 1));
                    }
                }
                if (currentPoint.X - 1 > 0 && currentPoint.X - 1 < image.Width && currentPoint.Y > 0 && currentPoint.Y < image.Height)
                {
                    Color leftPixel = image.GetPixel(currentPoint.X - 1, currentPoint.Y);
                    if (leftPixel.ToArgb() != borderColor.ToArgb() && leftPixel.ToArgb() != color.ToArgb())
                    {
                        points.Push(new Point(currentPoint.X - 1, currentPoint.Y));
                    }
                }
            }

            return image;
        }



        Bitmap floodFill(Bitmap sourceImage, int x, int y, Color oldcolor, Color newcolor)
        {
            Bitmap bmp = (Bitmap)sourceImage.Clone();            

            Stack<Point> stackPixels = new Stack<Point>();

            //1.Поместить затравочный пиксел в стек;
            stackPixels.Push(new Point(x, y));

            Point currentPixel;
            do
            {
                //2.Извлечь пиксел из стека;
                currentPixel = stackPixels.Pop();
                //3.Присвоить пикселу требуемое значение(цвет внутренней области);
                bmp.SetPixel(currentPixel.X, currentPixel.Y, newcolor);
                //gr.DrawLine(currentPen, new Point(currentPixel.X, currentPixel.Y), new Point(currentPixel.X, currentPixel.Y));


                //grPanel.DrawLine(currentPen, currentPixel.X, currentPixel.Y, currentPixel.X, currentPixel.Y);

                // 4.Каждый окрестный пиксел добавить в стек, если он

                //4.1.Не является граничным;                
                if (currentPixel.X - 1 > 0 && currentPixel.X - 1 < bmp.Width && currentPixel.Y > 0 && currentPixel.Y < bmp.Height)
                {
                    //4.2.Не обработан ранее(т.е.его цвет отличается от цвета границы или цвета внутренней области);                   
                    if (bmp.GetPixel(currentPixel.X - 1, currentPixel.Y).ToArgb() == oldcolor.ToArgb())
                    {
                        stackPixels.Push(new Point(currentPixel.X - 1, currentPixel.Y));
                    }
                }

                if (currentPixel.X + 1 > 0 && currentPixel.X + 1 < bmp.Width && currentPixel.Y > 0 && currentPixel.Y < bmp.Height)
                {
                    if (bmp.GetPixel(currentPixel.X + 1, currentPixel.Y).ToArgb() == oldcolor.ToArgb())
                    {
                        stackPixels.Push(new Point(currentPixel.X + 1, currentPixel.Y));
                    }
                }

                if (currentPixel.X > 0 && currentPixel.X < bmp.Width && currentPixel.Y - 1 > 0 && currentPixel.Y - 1 < bmp.Height)
                {
                    if (bmp.GetPixel(currentPixel.X, currentPixel.Y - 1).ToArgb() == oldcolor.ToArgb())
                    {
                        stackPixels.Push(new Point(currentPixel.X, currentPixel.Y - 1));
                    }
                }

                if (currentPixel.X > 0 && currentPixel.X < bmp.Width && currentPixel.Y + 1 > 0 && currentPixel.Y + 1 < bmp.Height)
                {
                    if (bmp.GetPixel(currentPixel.X, currentPixel.Y + 1).ToArgb() == oldcolor.ToArgb())
                    {
                        stackPixels.Push(new Point(currentPixel.X, currentPixel.Y + 1));
                    }
                }
            } while (stackPixels.Count != 0); //5.Если стек не пуст, перейти к шагу 2    

            return bmp;
        }

        void BresenhamCircle(int x0, int y0, int radius)
        {
            Graphics p = Graphics.FromHwnd(panelPaint.Handle);

            int x = radius;
            int y = 0;
            int radiusError = 1 - x;
            while (x >= y)
            {
                p.FillRectangle(new SolidBrush(CurrentColor), x + x0, y + y0, currentPen.Width, currentPen.Width);
                p.FillRectangle(new SolidBrush(CurrentColor), y + x0, x + y0, currentPen.Width, currentPen.Width);
                p.FillRectangle(new SolidBrush(CurrentColor), -x + x0, y + y0, currentPen.Width, currentPen.Width);
                p.FillRectangle(new SolidBrush(CurrentColor), -y + x0, x + y0, currentPen.Width, currentPen.Width);
                p.FillRectangle(new SolidBrush(CurrentColor), -x + x0, -y + y0, currentPen.Width, currentPen.Width);
                p.FillRectangle(new SolidBrush(CurrentColor), -y + x0, -x + y0, currentPen.Width, currentPen.Width);
                p.FillRectangle(new SolidBrush(CurrentColor), x + x0, -y + y0, currentPen.Width, currentPen.Width);
                p.FillRectangle(new SolidBrush(CurrentColor), y + x0, -x + y0, currentPen.Width, currentPen.Width);                
                y++;
                if (radiusError < 0)
                {
                    radiusError += 2 * y + 1;
                }
                else
                {
                    x--;
                    radiusError += 2 * (y - x + 1);
                }
            }
        }

        void BresenhamLine(int x0, int y0, int x1, int y1)
        {
            Graphics p = Graphics.FromHwnd(panelPaint.Handle);

            var steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0); // Проверяем рост отрезка по оси икс и по оси игрек
                                                               // Отражаем линию по диагонали, если угол наклона слишком большой
            if (steep)
            {
                Swap(ref x0, ref y0); // Перетасовка координат вынесена в отдельную функцию для красоты
                Swap(ref x1, ref y1);
            }
            // Если линия растёт не слева направо, то меняем начало и конец отрезка местами
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }
            int dx = x1 - x0;
            int dy = Math.Abs(y1 - y0);
            int error = dx / 2; // Здесь используется оптимизация с умножением на dx, чтобы избавиться от лишних дробей
            int ystep = (y0 < y1) ? 1 : -1; // Выбираем направление роста координаты y
            int y = y0;
            for (int x = x0; x <= x1; x++)
            {
                p.FillRectangle(new SolidBrush(CurrentColor), steep ? y : x, steep ? x : y, currentPen.Width, currentPen.Width); // Не забываем вернуть координаты на место
                error -= dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
        }

        void Swap(ref int t1, ref int t2)
        {
            int temp = t1;
            t1 = t2;
            t2 = temp;
        }

        public void drawesLines(Point CurrentPoint_1, Point CurrentPoint_2)
        {
            Graphics p = Graphics.FromHwnd(panelPaint.Handle);

            if (CurrentPoint_1.X > CurrentPoint_2.X)
            {
                for (double i = CurrentPoint_1.X; i >= CurrentPoint_2.X; i -= 0.01)
                {
                    //TimeDrawing.BegDraw(); // time adjustment(beg draw line)

                    double yN = (CurrentPoint_2.Y - CurrentPoint_1.Y) * (i - CurrentPoint_1.X) / (CurrentPoint_2.X - CurrentPoint_1.X) + CurrentPoint_1.Y;

                    p.FillRectangle(new SolidBrush(CurrentColor), (float)i, (float)yN, currentPen.Width, currentPen.Width);

                    //TimeDrawing.EndDraw(); // time adjustment(end draw line)
                }
            }
            else
            {
                for (double i = CurrentPoint_1.X; i <= CurrentPoint_2.X; i += 0.01)
                {
                    //TimeDrawing.BegDraw(); // time adjustment(beg draw line)

                    double yN = (CurrentPoint_2.Y - CurrentPoint_1.Y) * (i - CurrentPoint_1.X) / (CurrentPoint_2.X - CurrentPoint_1.X) + CurrentPoint_1.Y;
                    p.FillRectangle(new SolidBrush(CurrentColor), (float)i, (float)yN, currentPen.Width, currentPen.Width);

                    //TimeDrawing.EndDraw(); // time adjustment(end draw line)
                }

                
            }


        }

        public void drawesLinesY(Point CurrentPoint_1, Point CurrentPoint_2)
        {

            Graphics p = Graphics.FromHwnd(panelPaint.Handle);

            if (CurrentPoint_1.Y >= CurrentPoint_2.Y)
            {
                for (double i = CurrentPoint_1.Y; i >= CurrentPoint_2.Y; i -= 0.01)
                {
                   // TimeDrawing.BegDraw(); // time adjustment(beg draw line)

                    double xN = (CurrentPoint_2.X - CurrentPoint_1.X) * (i - CurrentPoint_1.Y) / (CurrentPoint_2.Y - CurrentPoint_1.Y) + CurrentPoint_1.X;
                    p.FillRectangle(new SolidBrush(CurrentColor), (float)xN, (float)i, currentPen.Width, currentPen.Width);

                    //TimeDrawing.EndDraw(); // time adjustment(end draw line)
                }
            }
            else
            {
                for (double i = CurrentPoint_1.Y; i <= CurrentPoint_2.Y; i += 0.01)
                {
                    //TimeDrawing.BegDraw(); // time adjustment(beg draw line)

                    double xN = (CurrentPoint_2.X - CurrentPoint_1.X) * (i - CurrentPoint_1.Y) / (CurrentPoint_2.Y - CurrentPoint_1.Y) + CurrentPoint_1.X;
                    p.FillRectangle(new SolidBrush(CurrentColor), (float)xN, (float)i, currentPen.Width, currentPen.Width);

                    //TimeDrawing.EndDraw(); // time adjustment(end draw line)
                }
            }

        }

        public void drawesCircles(Point CurrentPoint_1, double r)
        {
            Graphics p = Graphics.FromHwnd(panelPaint.Handle);

            for (double i = CurrentPoint_1.X; i <= CurrentPoint_1.X + r; i += 0.01)
            {
                double yN = Math.Abs(Math.Sqrt(r * r - Math.Pow((i - CurrentPoint_1.X), 2.0)) + CurrentPoint_1.Y);
                p.FillRectangle(new SolidBrush(CurrentColor), (float)i, (float)yN, currentPen.Width, currentPen.Width);
            }

            for (double i = CurrentPoint_1.X + r; i >= CurrentPoint_1.X; i -= 0.01)
            {
                double yN = CurrentPoint_1.Y - Math.Abs(Math.Sqrt(r * r - Math.Pow((i - CurrentPoint_1.X), 2.0)));
                p.FillRectangle(new SolidBrush(CurrentColor), (float)i, (float)yN, currentPen.Width, currentPen.Width);
            }

            for (double i = CurrentPoint_1.X; i >= CurrentPoint_1.X - r; i -= 0.01)
            {
                double yN = CurrentPoint_1.Y + Math.Sqrt(r * r - Math.Pow((i - CurrentPoint_1.X), 2.0));
                p.FillRectangle(new SolidBrush(CurrentColor), (float)i, (float)yN, currentPen.Width, currentPen.Width);
            }


            for (double i = CurrentPoint_1.X - r; i <= CurrentPoint_1.X; i += 0.01)
            {
                double yN = CurrentPoint_1.Y - Math.Sqrt(r * r - Math.Pow((i - CurrentPoint_1.X), 2.0));
                p.FillRectangle(new SolidBrush(CurrentColor), (float)i, (float)yN, currentPen.Width, currentPen.Width);
            }



        }
        public Form1()
        {

            InitializeComponent();
            g = panelPaint.CreateGraphics();
            currentPen = new Pen(CurrentColor, float.Parse(tBoxThicknessLine.Text));
            MillisecOnDote = 1000 / (int)(uint.Parse(tBoxDotesPerSec.Text));
            timer.Interval = MillisecOnDote;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            ifPressed = false;
            //ifPressed1 = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (ifPressed == true)
            {
                PrevPoint = CurrentPoint;
                CurrentPoint = e.Location;
                for_paint();

            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ifPressed = true;
            CurrentPoint = e.Location;
        } 
       
        

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (isPouring) // mode of pouring
            {
                // bit map of panel
                Bitmap bmpOfPanel = new Bitmap(this.panelPaint.Width, this.panelPaint.Height);
                Graphics grBitMap = Graphics.FromImage(bmpOfPanel);
                Rectangle rect = panelPaint.RectangleToScreen(panelPaint.ClientRectangle);
                grBitMap.CopyFromScreen(rect.Location, Point.Empty, panelPaint.ClientSize);

                //// get new bitmap with pouring                
                //Bitmap newImageForPanel = floodFill(bmpOfPanel, e.Location.X, e.Location.Y, bmpOfPanel.GetPixel(e.Location.X, e.Location.Y), ColorPouring);
                //    //PaintZone(bmOfPanel, e.Location.X, e.Location.Y, ColorPouring, CurrentColor);
                //panelPaint.BackgroundImage = newImageForPanel;  

                taskCalcPoints = calcPointsAsync.PouringArea(bmpOfPanel, e.Location.X, e.Location.Y, bmpOfPanel.GetPixel(e.Location.X, e.Location.Y), ColorPouring);
                timer.Start();
              
                return;
            }

            if (pen == false)
            {

                if ((line == true) || (kr == true) || (thr == true) || (kvad == true))
                {
                    l = e.X;
                    m = e.Y;
                    //Pen myPen = new Pen(CurrentColor, 5);
                    Graphics p = Graphics.FromHwnd(panelPaint.Handle);
                    p.DrawEllipse(currentPen, l, m, currentPen.Width, currentPen.Width);
                    p.FillEllipse(new SolidBrush(CurrentColor), l, m, currentPen.Width, currentPen.Width);
                }


                counter++;
                switch (counter)
                {
                    case 1:
                        CurrentPoint_2 = CurrentPoint_1; //++
                        CurrentPoint_1 = e.Location; //++ 
                        x = e.X;
                        y = e.Y;
                        break;



                    case 3:
                        CurrentPoint_4 = CurrentPoint_3; //++
                        CurrentPoint_3 = e.Location; //++ 
                        break;

                }
            }

            CurrentPoint_3 = CurrentPoint_2;
            CurrentPoint_2 = e.Location;


        }

        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void butColor_Click(object sender, EventArgs e)
        {
            resetPouring();
            resetCursorOfPanel();

            DialogResult D = colorDialog1.ShowDialog();
            if (D == System.Windows.Forms.DialogResult.OK)
            {
                CurrentColor = colorDialog1.Color;
                currentPen.Color = CurrentColor;
            }
        }

        private void butLine_Click(object sender, EventArgs e)
        {
            resetCursorOfPanel();
            resetPouring();

            line = true;
            //MessageBox.Show("Вы выбрали инструмент - <Линия>. Для того что бы построить линию, вам нужно выбрать цвет(по желанию), поставить две точки на области для рисования, и нажать кнопку <Построить>.");
            //panel1.Refresh();
            Point CurrentPoint_1 = new Point(0, 0);
            Point CurrentPoint_2 = new Point(0, 0);
            Point CurrentPoint_3 = new Point(0, 0);
            Point CurrentPoint_4 = new Point(0, 0);
            Point CurrentPoint_5 = new Point(0, 0);
            pen = false;
            counter = 0;
            l = 0;
            m = 0;
            x = 0;
            y = 0;

            butBuildChoseFigure.Focus();
        }

        private void butCircle_Click(object sender, EventArgs e)
        {
            resetCursorOfPanel();
            resetPouring();

            kr = true;
            //MessageBox.Show("Вы выбрали инструмент - <Круг>. Для того что бы построить круг, вам нужно выбрать цвет(по желанию), поставить левую верхнюю границу круга на области для рисования, и нажать кнопку <Построить>.");
            //panel1.Refresh();

            FormRadiusOfCircle formRadiusCircle = new FormRadiusOfCircle(); // form for get size of square
            formRadiusCircle.StartPosition = FormStartPosition.CenterParent;
            formRadiusCircle.ShowDialog();

            RadiusOfCircle = formRadiusCircle.RadiusOfCircle;

            Point CurrentPoint_1 = new Point(0, 0);
            Point CurrentPoint_2 = new Point(0, 0);
            Point CurrentPoint_3 = new Point(0, 0);
            Point CurrentPoint_4 = new Point(0, 0);
            Point CurrentPoint_5 = new Point(0, 0);
            pen = false;
            counter = 0;
            l = 0;
            m = 0;
            x = 0;
            y = 0;

            butBuildChoseFigure.Select();
        }

        private void butTriangle_Click(object sender, EventArgs e)
        {
            resetCursorOfPanel();
            resetPouring();

            //MessageBox.Show("Вы выбрали инструмент - <Треугольник>. Для того что бы построить треугольник, вам нужно выбрать цвет(по желанию), поставить три точки на области для рисования, и нажать кнопку <Построить>.");
            thr = true;
            // panel1.Refresh();
            Point CurrentPoint_1 = new Point(0, 0);
            Point CurrentPoint_2 = new Point(0, 0);
            Point CurrentPoint_3 = new Point(0, 0);
            Point CurrentPoint_4 = new Point(0, 0);
            Point CurrentPoint_5 = new Point(0, 0);
            pen = false;
            counter = 0;
            l = 0;
            m = 0;
            x = 0;
            y = 0;

            butBuildChoseFigure.Focus();
        }

        private void for_paint()
        {
            if (pen == true)
            {
                //Pen p = new Pen(CurrentColor);
                g.DrawLine(currentPen, PrevPoint, CurrentPoint);
            }
        }


        private void butClear_Click(object sender, EventArgs e)
        {
            resetPouring();
            resetCursorOfPanel();

            if(taskCalcPoints != null) // stop draw figure and reset all fields
            {
                calculatedPoints = new Queue<Point>();
                calcPointsAsync.CancellationTokenSource.Cancel();
                timer.Stop();
            }

            panelPaint.Refresh();
            panelPaint.BackgroundImage = null;
            Point CurrentPoint_1 = new Point(0, 0);
            Point CurrentPoint_2 = new Point(0, 0);
            Point CurrentPoint_3 = new Point(0, 0);
            Point CurrentPoint_4 = new Point(0, 0);
            Point CurrentPoint_5 = new Point(0, 0);
            pen = false;
            counter = 0;
            l = 0;
            m = 0;
            x = 0;
            y = 0;

        }

        private async void GetPointsForDraw()
        {
            
        }

        private void butBuildFigure_Click(object sender, EventArgs e)
        {
            resetPouring();


            if (line == true)
            {

                //drawesLines(CurrentPoint_1, CurrentPoint_2);
                //BresenhamLine(CurrentPoint_1.X, CurrentPoint_1.Y, CurrentPoint_2.X, CurrentPoint_2.Y);

                taskCalcPoints = calcPointsAsync.CalcPolygon(new List<Point>() { CurrentPoint_1, CurrentPoint_2 });
                timer.Start();

                line = false;
            }
            if (thr == true)
            {
                taskCalcPoints = calcPointsAsync.CalcPolygon(new List<Point>() { CurrentPoint_1, CurrentPoint_2, CurrentPoint_3, CurrentPoint_4 });
                timer.Start();
                //BresenhamLine(CurrentPoint_1.X, CurrentPoint_1.Y, CurrentPoint_2.X, CurrentPoint_2.Y);
                //BresenhamLine(CurrentPoint_2.X, CurrentPoint_2.Y, CurrentPoint_3.X, CurrentPoint_3.Y);
                //BresenhamLine(CurrentPoint_3.X, CurrentPoint_3.Y, CurrentPoint_4.X, CurrentPoint_4.Y);
                ////drawesLines(CurrentPoint_1, CurrentPoint_2);
                //drawesLines(CurrentPoint_2, CurrentPoint_3);
                //drawesLines(CurrentPoint_3, CurrentPoint_4);

                thr = false;
            }
            if (kr == true)
            {
                taskCalcPoints = calcPointsAsync.CalcCircle(new Circle() { center = new Point(x, y), radius = RadiusOfCircle});
                timer.Start();


                //drawesCircles(new Point(x, y), RadiusOfCircle);
                //BresenhamCircle(x, y, RadiusOfCircle);
                //kr = false;               
            }
            if (kvad == true)
            {
                taskCalcPoints = calcPointsAsync.CalcPolygon(new List<Point>()
                {
                    new Point(x, y),
                    new Point(x + SizeSideSquare, y),
                    new Point(x + SizeSideSquare, y),
                    new Point(x + SizeSideSquare, y + SizeSideSquare),
                    new Point(x + SizeSideSquare, y + SizeSideSquare),
                    new Point(x, y + SizeSideSquare),
                    new Point(x, y + SizeSideSquare),
                    new Point(x, y)
                });
                timer.Start();
                //BresenhamLine(x, y, x + SizeSideSquare, y);
                //BresenhamLine(x + SizeSideSquare, y, x + SizeSideSquare, y + SizeSideSquare);
                //BresenhamLine(x + SizeSideSquare, y + SizeSideSquare, x, y + SizeSideSquare);
                //BresenhamLine(x, y + SizeSideSquare,x, y);
                //drawesLines(new Point(x, y), new Point(x + SizeSideSquare, y));
                //drawesLinesY(new Point(x + SizeSideSquare, y), new Point(x + SizeSideSquare, y + SizeSideSquare));
                //drawesLines(new Point(x + SizeSideSquare, y + SizeSideSquare), new Point(x, y + SizeSideSquare));
                //drawesLinesY(new Point(x, y + SizeSideSquare), new Point(x, y));
                kvad = false;
            }
        }

        private void butPencil_Click(object sender, EventArgs e)
        {
            pen = true;

            resetCursorOfPanel();
            resetPouring();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {

            MessageBoxButtons msb = MessageBoxButtons.YesNo;
            String message = "Вы действительно хотите выйти?";
            String caption = "Выход";
            if (MessageBox.Show(message, caption, msb) == DialogResult.Yes)
                this.Close();

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void tBoxThicknessLine_TextChanged(object sender, EventArgs e)
        {
            if (tBoxThicknessLine.Text == string.Empty) // wait input
                return;

            try
            {
                float widthPen = float.Parse(tBoxThicknessLine.Text);
                if (widthPen < 0)
                    throw new Exception("Not valid size of line");

                currentPen.Width = widthPen;
                
            }
            catch (Exception ex)
            {
                tBoxThicknessLine.Text = currentPen.Width.ToString();
                MessageBox.Show("Not valid size of line");
            }
        }

        private void butPouring_Click(object sender, EventArgs e)
        {
            // dialog choose color for pouring
            DialogResult D = colorDialog1.ShowDialog();
            if (D == System.Windows.Forms.DialogResult.OK)
            {
                ColorPouring = colorDialog1.Color;

                panelPaint.Cursor = Cursors.Cross; // change cursor for pouring
                isPouring = true; // set pouring
            }

        }

        private void resetPouring()
        {
            isPouring = false;
        }

        private void resetCursorOfPanel()
        {
            if (panelPaint.Cursor != Cursors.Arrow)
                panelPaint.Cursor = Cursors.Arrow;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // bit map of panel
            Bitmap bmpOfPanel = new Bitmap(this.panelPaint.Width, this.panelPaint.Height);
            Graphics grBitMap = Graphics.FromImage(bmpOfPanel);
            Rectangle rect = panelPaint.RectangleToScreen(panelPaint.ClientRectangle);
            grBitMap.CopyFromScreen(rect.Location, Point.Empty, panelPaint.ClientSize);

            // dialog for save
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "Bitmap Image (.bmp)|*.bmp|Gif Image (.gif)|*.gif|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png|Tiff Image (.tiff)|*.tiff|Wmf Image (.wmf)|*.wmf";
            sf.ShowDialog();
            var path = sf.FileName;           

            // save image           
            bmpOfPanel.Save(path);
        }

        private void panelPaint_Resize(object sender, EventArgs e)
        {
            g = panelPaint.CreateGraphics();
        }

        private void tBoxDotesPerSec_TextChanged(object sender, EventArgs e)
        {
            if (tBoxDotesPerSec.Text == string.Empty) // wait input
                return;

            try
            {
                DotesPerSec = uint.Parse(tBoxDotesPerSec.Text);

                MillisecOnDote = 1000 / (int)DotesPerSec;// millisec on dote
                if (MillisecOnDote < 1)
                    throw new Exception("Notvalid value of dotes per sec");
                timer.Interval = MillisecOnDote;
            }
            catch (Exception ex)
            {
                tBoxDotesPerSec.Text = DotesPerSecond.ToString();
                MessageBox.Show("Not valid value of dotes per sec");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(calculatedPoints == null || calculatedPoints.Count == 0) // empty or null accumulated points
            {
                if((calculatedPoints = calcPointsAsync.TryGetAccumulatedPoints()) == null && calcPointsAsync.EndCalcPoints) // end of calculated
                {
                    timer.Stop();
                    if(kr == true) // kostil bad draw circle
                    {
                        BresenhamCircle(x, y, RadiusOfCircle);
                        kr = false;
                    }
                }
                else if(calculatedPoints == null) // not calc already
                {
                    //if (timer.Interval < 10) // some increase interval for calc
                       //timer.Interval += 40;
                }
            }
            else // get and draw point
            {                
                Point point = calculatedPoints.Dequeue();
                if(isPouring) // pouring by pixel
                {
                    g.FillRectangle(new SolidBrush(ColorPouring), point.X, point.Y, 1, 1);
                }
                else // draw pen
                {
                    g.FillRectangle(new SolidBrush(CurrentColor), point.X, point.Y, currentPen.Width, currentPen.Width);
                }               
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //Graphics k = Graphics.FromImage(bmp);

        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            butSaveImage.PerformClick();
        }

        private void разработчикToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программу разработал Михайленко Александр. ДНУЗЖТ 2018.");
        }

        private void контактыToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("<@mixajlenko>");
        }

        private void butSquare_Click(object sender, EventArgs e)
        {
            resetCursorOfPanel();
            resetPouring();

            kvad = true;
            //MessageBox.Show("Вы выбрали инструмент - <Квадрат>. Для того что бы построить квадрат, вам нужно выбрать цвет(по желанию), поставить одну точку, которая будет определять левый верхний угол квадрата на области для рисования и выбрать длину стороны квадрата, и нажать кнопку <Построить>.");

            FormSizeOfSquare formSizeSquare = new FormSizeOfSquare(); // form for get size of square            
            formSizeSquare.StartPosition = FormStartPosition.CenterParent;
            formSizeSquare.ShowDialog();

            SizeSideSquare = formSizeSquare.SizeOfSideSquare;

            //Point pointOfForm = this.Location;
            //pointOfForm.X += this.Width ;
            //pointOfForm.Y += this.Height;
            //formSizeSquare.Location = pointOfForm;


            //panel1.Refresh();
            Point CurrentPoint_1 = new Point(0, 0);
            Point CurrentPoint_2 = new Point(0, 0);
            Point CurrentPoint_3 = new Point(0, 0);
            Point CurrentPoint_4 = new Point(0, 0);
            Point CurrentPoint_5 = new Point(0, 0);
            pen = false;
            counter = 0;
            l = 0;
            m = 0;
            x = 0;
            y = 0;

            butBuildChoseFigure.Focus();
        }


    }
}
