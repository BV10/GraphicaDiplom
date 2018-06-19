using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//TODO: растровая заливка
//HACK: круг - радиус
//HACK: квадрат - стороны
//TODO: скорость отрисовки
//HACK: несколько фигур на полотне
//HACK: регулировка толщины линий
//TODO: сохранять изображения
//TODO: масштабирование
//TODO: окошко для демонстрации работы
//UNDONE: срок -пятница
//TODO: количество точек за секунду
//TODO: качественнее круг
//HACK: побольше панель

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        private int SizeSideSquare { get; set; }
        public int RadiusOfCircle { get; private set; }

        Color CurrentColor = Color.Black;        
        bool ifPressed = false;
        bool ifPressed1 = false;
        bool  pen = false;
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
        Point PrevPoint_1;
        Point PrevPoint_2;
        int counter = 0;
        int x, y,l,m,k,s;
        Graphics g;
        Bitmap bmp;
        Pen currentPen;

        bool isPouring = false;

        void floodFill(int x, int y, Color oldcolor, Color newcolor)
        {        

            Stack<Point> stackPixels = new Stack<Point>();  

            //1.Поместить затравочный пиксел в стек;
            stackPixels.Push(new Point(x, y));

            Point currentPixel;
            do
            {
                //2.Извлечь пиксел из стека;
                currentPixel = stackPixels.Pop();
                //3.Присвоить пикселу требуемое значение(цвет внутренней области);
                ////bmp.SetPixel(currentPixel.X, currentPixel.Y, newcolor);
                //gr.DrawLine(currentPen, new Point(currentPixel.X, currentPixel.Y), new Point(currentPixel.X, currentPixel.Y));
                

                //grPanel.DrawLine(currentPen, currentPixel.X, currentPixel.Y, currentPixel.X, currentPixel.Y);

                // 4.Каждый окрестный пиксел добавить в стек, если он

                //4.1.Не является граничным;                
                if (currentPixel.X-1 > 0 && currentPixel.X-1 < bmp.Width && currentPixel.Y > 0 && currentPixel.Y < bmp.Height)
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
        }

        public void drawesLines(Point CurrentPoint_1, Point CurrentPoint_2)
        {

            Graphics p = Graphics.FromHwnd(panelPaint.Handle);

            if (CurrentPoint_1.X > CurrentPoint_2.X)
            {

                for (double i = CurrentPoint_1.X; i >= CurrentPoint_2.X; i -= 0.01)
                {
                    double yN = (CurrentPoint_2.Y - CurrentPoint_1.Y) * (i - CurrentPoint_1.X) / (CurrentPoint_2.X - CurrentPoint_1.X) + CurrentPoint_1.Y;
                    p.FillRectangle(new SolidBrush(CurrentColor), (float)i, (float)yN, currentPen.Width, currentPen.Width);
                }
            }
            else
            {
                for (double i = CurrentPoint_1.X; i <= CurrentPoint_2.X; i += 0.01)
                {
                    double yN = (CurrentPoint_2.Y - CurrentPoint_1.Y) * (i - CurrentPoint_1.X) / (CurrentPoint_2.X - CurrentPoint_1.X) + CurrentPoint_1.Y;
                    p.FillRectangle(new SolidBrush(CurrentColor), (float)i, (float)yN, currentPen.Width, currentPen.Width);
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
                    double xN = (CurrentPoint_2.X - CurrentPoint_1.X) * (i - CurrentPoint_1.Y) / (CurrentPoint_2.Y - CurrentPoint_1.Y) + CurrentPoint_1.X;
                    p.FillRectangle(new SolidBrush(CurrentColor), (float)xN, (float)i, currentPen.Width, currentPen.Width);
                }
            }
            else
            {
                for (double i = CurrentPoint_1.Y; i <= CurrentPoint_2.Y; i += 0.01)
                {
                    double xN = (CurrentPoint_2.X - CurrentPoint_1.X) * (i - CurrentPoint_1.Y) / (CurrentPoint_2.Y - CurrentPoint_1.Y) + CurrentPoint_1.X;
                    p.FillRectangle(new SolidBrush(CurrentColor), (float)xN, (float)i, currentPen.Width, currentPen.Width);
                }
            }
            
        }

        public void drawesCircles(Point CurrentPoint_1, double r)
        {
            Graphics p = Graphics.FromHwnd(panelPaint.Handle);

            for (double i = CurrentPoint_1.X; i <= CurrentPoint_1.X + r; i += 0.001)
            {
                double yN = Math.Abs(Math.Sqrt(r * r - Math.Pow((i - CurrentPoint_1.X), 2.0)) + CurrentPoint_1.Y);
                p.FillRectangle(new SolidBrush(CurrentColor), (float)i, (float)yN, currentPen.Width, currentPen.Width);
            }

            for (double i = CurrentPoint_1.X + r; i >= CurrentPoint_1.X; i -= 0.001)
            {
                double yN = CurrentPoint_1.Y - Math.Abs(Math.Sqrt(r * r - Math.Pow((i - CurrentPoint_1.X), 2.0)));
                p.FillRectangle(new SolidBrush(CurrentColor), (float)i, (float)yN, currentPen.Width, currentPen.Width);
            }

            for (double i = CurrentPoint_1.X; i >= CurrentPoint_1.X - r; i -= 0.001)
            {
                double yN = CurrentPoint_1.Y + Math.Sqrt(r * r - Math.Pow((i - CurrentPoint_1.X), 2.0));
                p.FillRectangle(new SolidBrush(CurrentColor), (float)i, (float)yN, currentPen.Width, currentPen.Width);
            }


            for (double i = CurrentPoint_1.X - r; i <= CurrentPoint_1.X; i += 0.001)
            {
                double yN = CurrentPoint_1.Y  - Math.Sqrt(r * r - Math.Pow((i - CurrentPoint_1.X), 2.0)) ;
                p.FillRectangle(new SolidBrush(CurrentColor), (float)i, (float)yN, currentPen.Width, currentPen.Width);
            }



        }
        public Form1()
        {
            
            InitializeComponent();
            g = panelPaint.CreateGraphics();            
            currentPen = new Pen(CurrentColor, float.Parse(tBoxThicknessLine.Text));         
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            ifPressed = false;
            ifPressed1 = false;
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
            if(isPouring) // mode of pouring
            {
                bmp = new Bitmap(panelPaint.Width, panelPaint.Height);
                panelPaint.DrawToBitmap(bmp, new Rectangle(0, 0, panelPaint.Width, panelPaint.Height));

                floodFill(e.Location.X, e.Location.Y,  bmp.GetPixel(e.Location.X, e.Location.Y), CurrentColor);
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
                    case 1: CurrentPoint_2 = CurrentPoint_1; //++
                        CurrentPoint_1 = e.Location; //++ 
                        x = e.X;
                        y = e.Y;
                        break;

                    

                    case 3: CurrentPoint_4 = CurrentPoint_3; //++
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

            FormRadiusOfCircle formSizeSquare = new FormRadiusOfCircle(); // form for get size of square
            formSizeSquare.ShowDialog();

            RadiusOfCircle = formSizeSquare.RadiusOfCircle;

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

            panelPaint.Refresh();
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

        private void butBuildFigure_Click(object sender, EventArgs e)
        {
            resetPouring();


            if (line == true)
            {
                
                drawesLines(CurrentPoint_1, CurrentPoint_2);

                line = false;
            }
            if (thr == true)
            {
                drawesLines(CurrentPoint_1, CurrentPoint_2);
                drawesLines(CurrentPoint_2, CurrentPoint_3);
                drawesLines(CurrentPoint_3, CurrentPoint_4);
                
                thr = false;
            }
            if (kr == true)
            {
                drawesCircles(new Point(x, y), RadiusOfCircle);
                kr = false;
            }
            if (kvad == true)
            {
                drawesLines(new Point(x, y),new Point(x+SizeSideSquare, y));
                drawesLinesY(new Point(x + SizeSideSquare, y), new Point(x + SizeSideSquare, y+ SizeSideSquare));
                drawesLines(new Point(x + SizeSideSquare, y + SizeSideSquare), new Point(x, y + SizeSideSquare));
                drawesLinesY(new Point(x, y + SizeSideSquare), new Point(x, y));
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
            bmp = new Bitmap(panelPaint.Width, panelPaint.Height);
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
            try
            {
                currentPen.Width = float.Parse(tBoxThicknessLine.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Not valid size of line");
            }
        }

        private void butPouring_Click(object sender, EventArgs e)
        {
            panelPaint.Cursor = Cursors.Cross;
            isPouring = true;
        }

        private void resetPouring()
        {           
           isPouring = false;
        }

        private void resetCursorOfPanel()
        {
            if(panelPaint.Cursor != Cursors.Arrow)
                panelPaint.Cursor = Cursors.Arrow;
        }

        private void panelPaint_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //Graphics k = Graphics.FromImage(bmp);

        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(this.Width, this.Height);
            SaveFileDialog sfd = new SaveFileDialog();
            this.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                bmp.Save(sfd.FileName);
            }
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
