using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
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
//HACK: окошко для демонстрации работы
//TODO: scaling image
//UNDONE: срок -пятница
//HACK: количество точек за секунду
//HACK: качественнее круг
//HACK: побольше панель

namespace WindowsFormsApplication2
{
    public partial class MainForm : Form
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
        Figure currentFigure = Figure.None;
        Point PrevPoint;
        int counter = 0;
        int x, y, l, m;
        Graphics g;
        Pen currentPen;

        // description
        DescriptionWork descriptionWork = new DescriptionWork();

        // delay drawing
        Queue<Point> calculatedPoints = new Queue<Point>();
        CalcPointsFigureAsync calcPointsAsync = new CalcPointsFigureAsync();
        Task taskCalcPoints;
        private const int Max_Speed_Draw_MillisecOnDote = 1;
        private int MillisecOnDote { get; set; }

        //scaling

        private const float koefZoomIn = 1.3f;
        private const float koefZoomOut = 0.85f;

        // object on image box

        bool isPouring = false;

        private Color colorBeforePouring; // for fast pouring after slowly fill
        private Point lastFillPoint; // for fast pouring after slowly fill
        private float zoom = 1f;

        public MainForm()
        {

            InitializeComponent();
            g = pictureBoxImage.CreateGraphics();
            currentPen = new Pen(CurrentColor, float.Parse(tBoxThicknessLine.Text));
            // start value speed drawing
            MillisecOnDote = Max_Speed_Draw_MillisecOnDote;
            timer.Interval = MillisecOnDote;

            // description elems
            int heightPrevElems = 0;
            descriptionWork.Algorithm = new Label()
            {
                Location = new Point(panelPaint.Location.X + pictureBoxImage.Size.Width + 15, panelPaint.Location.Y + 20),
                AutoSize = true,
                Text = "Алгоритм Брезенхема:",
                Visible = false,
                TextAlign = ContentAlignment.MiddleCenter
            };
            heightPrevElems += descriptionWork.Algorithm.Size.Height;
            this.Controls.Add(descriptionWork.Algorithm);

            descriptionWork.CurrentFigure = new Label()
            {
                Location = new Point(panelPaint.Location.X + pictureBoxImage.Size.Width + 50, panelPaint.Location.Y + 20 + heightPrevElems + 5),
                AutoSize = true,
                Text = "Линия",
                Visible = false,
                TextAlign = ContentAlignment.MiddleCenter
            };
            heightPrevElems += descriptionWork.CurrentFigure.Size.Height;
            this.Controls.Add(descriptionWork.CurrentFigure);

            descriptionWork.DesriptDotesPerSec = new Label()
            {
                Location = new Point(panelPaint.Location.X + pictureBoxImage.Size.Width + 20, panelPaint.Location.Y + 20 + heightPrevElems + 5),
                AutoSize = true,
                Text = "Точек за секунду:",
                Visible = false,
                TextAlign = ContentAlignment.MiddleCenter
            };
            heightPrevElems += descriptionWork.DesriptDotesPerSec.Size.Height;
            this.Controls.Add(descriptionWork.DesriptDotesPerSec);

            descriptionWork.DotesPerSec = new Label()
            {
                Location = new Point(panelPaint.Location.X + pictureBoxImage.Size.Width + 50, panelPaint.Location.Y + 20 + heightPrevElems + 5),
                AutoSize = true,
                Text = "125",
                Visible = false,
                TextAlign = ContentAlignment.MiddleCenter
            };
            heightPrevElems += descriptionWork.DotesPerSec.Size.Height;
            this.Controls.Add(descriptionWork.DotesPerSec);

            descriptionWork.DescrCurrentDote = new Label()
            {
                Location = new Point(panelPaint.Location.X + pictureBoxImage.Size.Width + 30, panelPaint.Location.Y + 20 + heightPrevElems + 5),
                AutoSize = true,
                Text = "Текущая точка:",
                Visible = false,
                TextAlign = ContentAlignment.MiddleCenter
            };
            heightPrevElems += descriptionWork.DescrCurrentDote.Size.Height;
            this.Controls.Add(descriptionWork.DescrCurrentDote);

            descriptionWork.CurrentDote = new Label()
            {
                Location = new Point(panelPaint.Location.X + pictureBoxImage.Size.Width + 25, panelPaint.Location.Y + 20 + heightPrevElems + 5),
                AutoSize = true,
                Text = "(3,5)",
                Visible = false,
                TextAlign = ContentAlignment.MiddleCenter
            };
            heightPrevElems += descriptionWork.CurrentDote.Size.Height;
            this.Controls.Add(descriptionWork.CurrentDote);
            // -------------------

            this.Controls.Add(descriptionWork.Algorithm);
        }

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
                    if (bmp.GetPixel(currentPixel.X - 1, currentPixel.Y).ToArgb() == oldcolor.ToArgb() && bmp.GetPixel(currentPixel.X - 1, currentPixel.Y).ToArgb() != newcolor.ToArgb())
                    {
                        stackPixels.Push(new Point(currentPixel.X - 1, currentPixel.Y));
                    }
                }

                if (currentPixel.X + 1 > 0 && currentPixel.X + 1 < bmp.Width && currentPixel.Y > 0 && currentPixel.Y < bmp.Height)
                {
                    if (bmp.GetPixel(currentPixel.X + 1, currentPixel.Y).ToArgb() == oldcolor.ToArgb() && bmp.GetPixel(currentPixel.X + 1, currentPixel.Y).ToArgb() != newcolor.ToArgb())
                    {
                        stackPixels.Push(new Point(currentPixel.X + 1, currentPixel.Y));
                    }
                }

                if (currentPixel.X > 0 && currentPixel.X < bmp.Width && currentPixel.Y - 1 > 0 && currentPixel.Y - 1 < bmp.Height)
                {
                    if (bmp.GetPixel(currentPixel.X, currentPixel.Y - 1).ToArgb() == oldcolor.ToArgb() && bmp.GetPixel(currentPixel.X, currentPixel.Y - 1).ToArgb() != newcolor.ToArgb())
                    {
                        stackPixels.Push(new Point(currentPixel.X, currentPixel.Y - 1));
                    }
                }

                if (currentPixel.X > 0 && currentPixel.X < bmp.Width && currentPixel.Y + 1 > 0 && currentPixel.Y + 1 < bmp.Height)
                {
                    if (bmp.GetPixel(currentPixel.X, currentPixel.Y + 1).ToArgb() == oldcolor.ToArgb() && bmp.GetPixel(currentPixel.X, currentPixel.Y + 1).ToArgb() != newcolor.ToArgb())
                    {
                        stackPixels.Push(new Point(currentPixel.X, currentPixel.Y + 1));
                    }
                }
            } while (stackPixels.Count != 0); //5.Если стек не пуст, перейти к шагу 2    

            return bmp;
        }

        void BresenhamCircle(int x0, int y0, int radius)
        {
            Graphics p = Graphics.FromHwnd(pictureBoxImage.Handle);

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
            Graphics p = Graphics.FromHwnd(pictureBoxImage.Handle);

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



        private void pictureBoxImage_MouseWheel(object sender, MouseEventArgs e)
        {
            Bitmap bmpOfPanel = new Bitmap(this.pictureBoxImage.ClientSize.Width, this.pictureBoxImage.ClientSize.Height);
            Graphics grBitMap = Graphics.FromImage(bmpOfPanel);
            Rectangle rect = pictureBoxImage.RectangleToScreen(pictureBoxImage.ClientRectangle);
            grBitMap.CopyFromScreen(rect.Location, Point.Empty, pictureBoxImage.ClientSize);
            pictureBoxImage.Image = bmpOfPanel;


            //Bitmap bmp = new Bitmap(originalBitmap, newSize);
            Size newSize;

            if (e.Delta < 0)
            {
                zoom = koefZoomIn;
                newSize = new Size((int)(bmpOfPanel.Width * zoom), (int)(bmpOfPanel.Height * zoom));
                //panelPaint.Scale(new SizeF(koefZoomIn, koefZoomIn));
            }
            else
            {
                zoom = koefZoomOut;
                newSize = new Size((int)(bmpOfPanel.Width * zoom), (int)(bmpOfPanel.Height * zoom));
                //panelPaint.Scale(new SizeF(koefZoomIn, koefZoomIn));
                //panelPaint.Scale(new SizeF(koefZoomOut, koefZoomOut));
            }

            Bitmap bmp = new Bitmap(bmpOfPanel, newSize);
            pictureBoxImage.Image = bmp;

            //pictureBoxImage.Width =
            // Convert.ToInt32(pictureBoxImage.Image.Width * zoom);
            //pictureBoxImage.Height =
            //         Convert.ToInt32(pictureBoxImage.Image.Height * zoom);
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
            descriptionWork.inVisibleLabels();
            resetCursorOfPanel();
            resetPouring();

            currentFigure = Figure.Line;
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
            descriptionWork.inVisibleLabels();
            resetCursorOfPanel();
            resetPouring();

            currentFigure = Figure.Circle;
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
            descriptionWork.inVisibleLabels();
            resetCursorOfPanel();
            resetPouring();

            //MessageBox.Show("Вы выбрали инструмент - <Треугольник>. Для того что бы построить треугольник, вам нужно выбрать цвет(по желанию), поставить три точки на области для рисования, и нажать кнопку <Построить>.");
            currentFigure = Figure.Triangle;
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

            if (taskCalcPoints != null) // stop draw figure and reset all fields
            {
                calculatedPoints = new Queue<Point>();
                calcPointsAsync.CancellationTokenSource.Cancel();
                timer.Stop();
            }

            pictureBoxImage.Refresh();
            pictureBoxImage.BackgroundImage = null;
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

            // some descript
            descriptionWork.Algorithm.Visible = true; // descr beg build
            if (currentFigure != Figure.None)
            {
                descriptionWork.CurrentFigure.Text = currentFigure.ToString();
                descriptionWork.CurrentFigure.Visible = true;
            }
            descriptionWork.DesriptDotesPerSec.Visible = true;
            descriptionWork.DotesPerSec.Text = labelDotesPerSec.Text;
            descriptionWork.DotesPerSec.Visible = true;

            switch (currentFigure)
            {

                case Figure.Line:
                    {
                        //descr figure
                        if (timer.Interval == Max_Speed_Draw_MillisecOnDote) // fast drawing
                        {
                            BresenhamLine(CurrentPoint_1.X, CurrentPoint_1.Y, CurrentPoint_2.X, CurrentPoint_2.Y);
                            currentFigure = Figure.None; // end of paint
                            timer.Stop();
                        }
                        else
                        {
                            // visible current point 
                            descriptionWork.DescrCurrentDote.Visible = true;

                            taskCalcPoints = calcPointsAsync.CalcPolygon(new List<Point>() { CurrentPoint_1, CurrentPoint_2 });
                            timer.Start();
                        }
                    }
                    break;
                case Figure.Triangle:
                    {
                        if (timer.Interval == Max_Speed_Draw_MillisecOnDote) // fast drawing
                        {
                            BresenhamLine(CurrentPoint_1.X, CurrentPoint_1.Y, CurrentPoint_2.X, CurrentPoint_2.Y);
                            BresenhamLine(CurrentPoint_2.X, CurrentPoint_2.Y, CurrentPoint_3.X, CurrentPoint_3.Y);
                            BresenhamLine(CurrentPoint_3.X, CurrentPoint_3.Y, CurrentPoint_4.X, CurrentPoint_4.Y);

                            currentFigure = Figure.None; // end of paint

                            timer.Stop();
                        }
                        else
                        {
                            // visible current point 
                            descriptionWork.DescrCurrentDote.Visible = true;

                            taskCalcPoints = calcPointsAsync.CalcPolygon(new List<Point>() { CurrentPoint_1, CurrentPoint_2, CurrentPoint_3, CurrentPoint_4 });
                            timer.Start();
                        }
                    }
                    break;
                case Figure.Square:
                    {
                        if (timer.Interval == Max_Speed_Draw_MillisecOnDote) // fast drawing
                        {
                            BresenhamLine(x, y, x + SizeSideSquare, y);
                            BresenhamLine(x + SizeSideSquare, y, x + SizeSideSquare, y + SizeSideSquare);
                            BresenhamLine(x + SizeSideSquare, y + SizeSideSquare, x, y + SizeSideSquare);
                            BresenhamLine(x, y + SizeSideSquare, x, y);

                            currentFigure = Figure.None; // end of paint

                            timer.Stop();
                        }
                        else
                        {
                            // visible current point 
                            descriptionWork.DescrCurrentDote.Visible = true;

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
                        }
                    }
                    break;
                case Figure.Circle:
                    {
                        if (timer.Interval == Max_Speed_Draw_MillisecOnDote) // fast drawing
                        {
                            BresenhamCircle(x, y, RadiusOfCircle);
                            currentFigure = Figure.None; // end of paint

                            timer.Stop();
                        }
                        else
                        {
                            // visible current point 
                            descriptionWork.DescrCurrentDote.Visible = true;

                            taskCalcPoints = calcPointsAsync.CalcCircle(new Circle() { center = new Point(x, y), radius = RadiusOfCircle });
                            timer.Start();
                        }
                    }
                    break;
            }

        }

        private void butPencil_Click(object sender, EventArgs e)
        {
            descriptionWork.inVisibleLabels();

            descriptionWork.CurrentFigure.Text = "Карандаш";
            descriptionWork.CurrentFigure.Visible = true;
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

                pictureBoxImage.Cursor = Cursors.Cross; // change cursor for pouring
                isPouring = true; // set pouring
            }

        }

        private void resetPouring()
        {
            isPouring = false;
        }

        private void resetCursorOfPanel()
        {
            if (pictureBoxImage.Cursor != Cursors.Arrow)
                pictureBoxImage.Cursor = Cursors.Arrow;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            // bit map of panel
            Bitmap bmpOfPanel = new Bitmap(this.pictureBoxImage.Width, this.pictureBoxImage.Height);
            Graphics grBitMap = Graphics.FromImage(bmpOfPanel);
            Rectangle rect = pictureBoxImage.RectangleToScreen(pictureBoxImage.ClientRectangle);
            grBitMap.CopyFromScreen(rect.Location, Point.Empty, pictureBoxImage.ClientSize);

            // dialog for save
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "Bitmap Image (.bmp)|*.bmp|Gif Image (.gif)|*.gif|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png|Tiff Image (.tiff)|*.tiff|Wmf Image (.wmf)|*.wmf";
            sf.ShowDialog();


            var path = sf.FileName;

            // save image 
            if (!string.IsNullOrEmpty(path))
            {
                bmpOfPanel.Save(path);
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer.Interval == Max_Speed_Draw_MillisecOnDote) // max speed drawing 
            {
                if (taskCalcPoints != null) // stop calc draw figure
                {
                    calculatedPoints = new Queue<Point>();
                    calcPointsAsync.CancellationTokenSource.Cancel();
                }

                if (isPouring) // perform fast pouring
                {
                    // bit map of panel
                    Bitmap bmpOfPanel = new Bitmap(this.pictureBoxImage.Width, this.pictureBoxImage.Height);
                    Graphics grBitMap = Graphics.FromImage(bmpOfPanel);
                    Rectangle rect = pictureBoxImage.RectangleToScreen(pictureBoxImage.ClientRectangle);
                    grBitMap.CopyFromScreen(rect.Location, Point.Empty, pictureBoxImage.ClientSize);

                    bmpOfPanel.SetPixel(lastFillPoint.X, lastFillPoint.Y, colorBeforePouring);

                    Bitmap newImageForPanel = floodFill(bmpOfPanel, lastFillPoint.X, lastFillPoint.Y, colorBeforePouring, ColorPouring);

                    pictureBoxImage.Image = newImageForPanel;

                    timer.Stop();// end drawing stop

                    descriptionWork.inVisibleLabels();
                }
                else // fast drawing
                {
                    butBuildChoseFigure.PerformClick(); // click for drawing
                    descriptionWork.inVisibleLabels();
                }
            }
            else
            {

                if (calculatedPoints == null || calculatedPoints.Count == 0) // empty or null accumulated points
                {
                    if ((calculatedPoints = calcPointsAsync.TryGetAccumulatedPoints()) == null && calcPointsAsync.EndCalcPoints) // end of calculated
                    {
                        //descriptionWork.inVisibleLabels();
                        timer.Stop();
                        currentFigure = Figure.None; // end drawing
                    }
                    else if (calculatedPoints == null) // not calc already
                    {
                        //if (timer.Interval < 10) // some increase interval for calc
                        //timer.Interval += 40;
                    }
                }
                else // get and draw point
                {
                    Point point = calculatedPoints.Dequeue();

                    // descript current point
                    descriptionWork.CurrentDote.Visible = true;
                    descriptionWork.CurrentDote.Text = point.ToString();

                    if (isPouring) // pouring by pixel
                    {
                        lastFillPoint = point;
                        g.FillRectangle(new SolidBrush(ColorPouring), point.X, point.Y, 1, 1);
                    }
                    else // draw pen
                    {
                        g.FillRectangle(new SolidBrush(CurrentColor), point.X, point.Y, currentPen.Width, currentPen.Width);
                    }
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

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if (hScrollDotesPerSec.Value > 200) // max speed drawing
            {
                // change label
                labelDotesPerSec.Text = "Maximum";
                // change timer
                MillisecOnDote = Max_Speed_Draw_MillisecOnDote;
                timer.Interval = MillisecOnDote;
            }
            else
            {
                // change timer
                timer.Interval = 1000 / hScrollDotesPerSec.Value;
                MillisecOnDote = timer.Interval;
                // change label
                labelDotesPerSec.Text = hScrollDotesPerSec.Value.ToString();
            }
        }

        private void pictureBoxImage_MouseDown(object sender, MouseEventArgs e)
        {
            ifPressed = true;
            CurrentPoint = e.Location;
        }

        private void pictureBoxImage_MouseClick(object sender, MouseEventArgs e)
        {
            if (isPouring) // mode of pouring
            {
                // add descript                
                descriptionWork.Algorithm.Visible = true; // descr beg build
                descriptionWork.CurrentFigure.Text = "Заливка";
                descriptionWork.CurrentFigure.Visible = true;

                descriptionWork.DesriptDotesPerSec.Visible = true;
                descriptionWork.DotesPerSec.Text = labelDotesPerSec.Text;
                descriptionWork.DotesPerSec.Visible = true;

                // bit map of panel
                Bitmap bmpOfPanel = new Bitmap(this.pictureBoxImage.Width, this.pictureBoxImage.Height);
                Graphics grBitMap = Graphics.FromImage(bmpOfPanel);
                Rectangle rect = pictureBoxImage.RectangleToScreen(pictureBoxImage.ClientRectangle);
                grBitMap.CopyFromScreen(rect.Location, Point.Empty, pictureBoxImage.ClientSize);

                // get new bitmap with pouring                


                if (timer.Interval == Max_Speed_Draw_MillisecOnDote) // fast drawing
                {
                    colorBeforePouring = bmpOfPanel.GetPixel(e.Location.X, e.Location.Y);
                    Bitmap newImageForPanel = floodFill(bmpOfPanel, e.Location.X, e.Location.Y, bmpOfPanel.GetPixel(e.Location.X, e.Location.Y), ColorPouring);
                    pictureBoxImage.Image = newImageForPanel;


                    timer.Stop();
                }
                else
                {
                    // visible current point 
                    descriptionWork.DescrCurrentDote.Visible = true;

                    colorBeforePouring = bmpOfPanel.GetPixel(e.Location.X, e.Location.Y);
                    taskCalcPoints = calcPointsAsync.PouringArea(bmpOfPanel, e.Location.X, e.Location.Y, bmpOfPanel.GetPixel(e.Location.X, e.Location.Y), ColorPouring);
                    timer.Start();
                }



                return;
            }

            if (pen == false)
            {

                if (currentFigure != Figure.None)
                {
                    l = e.X;
                    m = e.Y;
                    //Pen myPen = new Pen(CurrentColor, 5);
                    Graphics p = Graphics.FromHwnd(pictureBoxImage.Handle);
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

        private void pictureBoxImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (ifPressed == true)
            {
                PrevPoint = CurrentPoint;
                CurrentPoint = e.Location;
                if (pen)
                {
                    // current point
                    descriptionWork.DescrCurrentDote.Visible = true;
                    descriptionWork.CurrentDote.Visible = true;
                    descriptionWork.CurrentDote.Text = CurrentPoint.ToString();
                }
                
                for_paint();

            }
        }

        private void pictureBoxImage_MouseUp(object sender, MouseEventArgs e)
        {
            ifPressed = false;
        }

        private void pictureBoxImage_Resize(object sender, EventArgs e)
        {

        }

        private void pictureBoxImage_Resize_1(object sender, EventArgs e)
        {
            g = pictureBoxImage.CreateGraphics();
        }

        private void инструкцияToolStripMenuItem_Click(object sender, EventArgs e)
        {           
            Process.Start("notepad.exe", Directory.GetCurrentDirectory() + "\\Intstruction.txt");
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
            descriptionWork.inVisibleLabels();
            resetCursorOfPanel();
            resetPouring();

            currentFigure = Figure.Square;
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
