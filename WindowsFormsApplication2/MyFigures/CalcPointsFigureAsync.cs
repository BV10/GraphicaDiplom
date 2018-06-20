using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApplication2.MyFigures
{
    class CalcPointsFigureAsync
    {
        private const int Min_Accumulating_Points = 20;
        private readonly object block = new object(); // blocker

        public CancellationTokenSource CancellationTokenSource { get; set; }

        private bool endCalcPoints = false;


        private Queue<Point> pointsForUser = new Queue<Point>();

        private Queue<Point> pointsCalculating = new Queue<Point>();

        public bool EndCalcPoints
        {
            get
            {
                lock (block)
                {
                    return endCalcPoints;
                }
            }
            set
            {
                lock (block)
                {
                    endCalcPoints = value;
                }
            }
        }

        public async Task CalcPolygon(List<Point> points)
        {
            pointsForUser = new Queue<Point>();
            pointsCalculating = new Queue<Point>();
            EndCalcPoints = false; // not end of calc

            CancellationTokenSource = new CancellationTokenSource();
            var token = CancellationTokenSource.Token;

            await Task.Factory.StartNew(() => CalcPolygonPoints(points, token), token);
        }

        public async Task CalcCircle(Circle circle)
        {
            //Min_Accumulating_Points = 15;
            pointsForUser = new Queue<Point>();
            pointsCalculating = new Queue<Point>();
            EndCalcPoints = false; // not end of calc

            CancellationTokenSource = new CancellationTokenSource();
            var token = CancellationTokenSource.Token;

            await Task.Factory.StartNew(() => CalcCirclePoints(circle, token), token);
        }

        public async Task PouringArea(Bitmap bmp, int x, int y, Color oldcolor, Color newcolor)
        {
            pointsForUser = new Queue<Point>();
            pointsCalculating = new Queue<Point>();
            EndCalcPoints = false; // not end of calc

            CancellationTokenSource = new CancellationTokenSource();
            var token = CancellationTokenSource.Token;

            await Task.Factory.StartNew(() => CalcPouringArea(bmp, x, y, oldcolor, newcolor, token), token);
        }

        private void CalcPouringArea(Bitmap sourceImage, int x, int y, Color oldcolor, Color newcolor, CancellationToken token)
        {
            Bitmap bmp = (Bitmap)sourceImage.Clone();

            Stack<Point> stackPixels = new Stack<Point>();

            //1.Поместить затравочный пиксел в стек;
            stackPixels.Push(new Point(x, y));

            // save first point pixel at queue
            pointsCalculating.Enqueue(new Point(x, y));

            Point currentPixel;
            do
            {
                if (token.IsCancellationRequested) // cancel task
                {
                    EndCalcPoints = true;
                    token.ThrowIfCancellationRequested();
                }

                if (pointsCalculating.Count >= Min_Accumulating_Points) // accumulate 10 dotes add them to user
                {
                    lock (block)
                    {
                        for (int i = 0; i < Min_Accumulating_Points; i++)
                        {
                            pointsForUser.Enqueue(pointsCalculating.Dequeue()); // points for user
                        }
                    }
                }

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
                        pointsCalculating.Enqueue(new Point(currentPixel.X - 1, currentPixel.Y));
                    }
                }

                if (currentPixel.X + 1 > 0 && currentPixel.X + 1 < bmp.Width && currentPixel.Y > 0 && currentPixel.Y < bmp.Height)
                {
                    if (bmp.GetPixel(currentPixel.X + 1, currentPixel.Y).ToArgb() == oldcolor.ToArgb())
                    {
                        stackPixels.Push(new Point(currentPixel.X + 1, currentPixel.Y));
                        pointsCalculating.Enqueue(new Point(currentPixel.X + 1, currentPixel.Y));
                    }
                }

                if (currentPixel.X > 0 && currentPixel.X < bmp.Width && currentPixel.Y - 1 > 0 && currentPixel.Y - 1 < bmp.Height)
                {
                    if (bmp.GetPixel(currentPixel.X, currentPixel.Y - 1).ToArgb() == oldcolor.ToArgb())
                    {
                        stackPixels.Push(new Point(currentPixel.X, currentPixel.Y - 1));
                        pointsCalculating.Enqueue(new Point(currentPixel.X, currentPixel.Y - 1));
                    }
                }

                if (currentPixel.X > 0 && currentPixel.X < bmp.Width && currentPixel.Y + 1 > 0 && currentPixel.Y + 1 < bmp.Height)
                {
                    if (bmp.GetPixel(currentPixel.X, currentPixel.Y + 1).ToArgb() == oldcolor.ToArgb())
                    {
                        stackPixels.Push(new Point(currentPixel.X, currentPixel.Y + 1));
                        pointsCalculating.Enqueue(new Point(currentPixel.X, currentPixel.Y + 1));
                    }
                }
            } while (stackPixels.Count != 0); //5.Если стек не пуст, перейти к шагу 2   


            lock (block)
            {
                EndCalcPoints = true; // end of calculating

                for (int i = 0; i < pointsCalculating.Count; i++) // last points
                {
                    pointsForUser.Enqueue(pointsCalculating.Dequeue()); // points for user
                }
            }
        }

        private void CalcCirclePoints(Circle circle, CancellationToken token)
        {
            int x0 = circle.center.X;
            int y0 = circle.center.Y;
            int x = circle.radius;
            int y = 0;
            int radiusError = 1 - x;
            while (x >= y)
            {
                if (token.IsCancellationRequested) // cancel task
                {
                    EndCalcPoints = true;
                    token.ThrowIfCancellationRequested();
                }

                if (pointsCalculating.Count >= Min_Accumulating_Points) // accumulate 10 dotes add them to user
                {
                    lock (block)
                    {
                        for (int i = 0; i < Min_Accumulating_Points; i++)
                        {
                            pointsForUser.Enqueue(pointsCalculating.Dequeue()); // points for user
                        }
                    }
                }

                // add dote at points calc
                pointsCalculating.Enqueue(new Point(x + x0, y + y0));
                pointsCalculating.Enqueue(new Point(y + x0, x + y0));
                pointsCalculating.Enqueue(new Point(-x + x0, y + y0));
                pointsCalculating.Enqueue(new Point(-y + x0, x + y0));
                pointsCalculating.Enqueue(new Point(-x + x0, -y + y0));
                pointsCalculating.Enqueue(new Point(-y + x0, -x + y0));
                pointsCalculating.Enqueue(new Point(x + x0, -y + y0));
                pointsCalculating.Enqueue(new Point(y + x0, -x + y0));
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

            lock (block)
            {
                EndCalcPoints = true; // end of calculating

                for (int i = 0; i < pointsCalculating.Count; i++) // last points
                {
                    pointsForUser.Enqueue(pointsCalculating.Dequeue()); // points for user
                }
            }
        }

        // get points accumulated quantity
        public Queue<Point> TryGetAccumulatedPoints()
        {
            lock (block)
            {
                if (pointsForUser.Count >= Min_Accumulating_Points)
                {
                    Queue<Point> retPoints = new Queue<Point>();
                    if (pointsForUser.Count != 0)
                    {
                        for (int i = 0; i < Min_Accumulating_Points && i < pointsForUser.Count; i++) // accumulated points in result and return
                            retPoints.Enqueue(pointsForUser.Dequeue());
                    }
                    return retPoints;
                }
                else if (EndCalcPoints && pointsForUser.Count == 0) // empty
                {
                    return null;
                }
                else if (EndCalcPoints) // get last points
                {
                    Queue<Point> retPoints = new Queue<Point>();
                    for (int i = 0; i < pointsForUser.Count; i++) // accumulated points in result and return
                        retPoints.Enqueue(pointsForUser.Dequeue());
                    return retPoints;
                }

            }
            return null;
        }



        void CalcPolygonPoints(object points, CancellationToken cancellationToken)
        {
            List<Point> pointsPolyg = points as List<Point>;

            for (int iterListPoints = 0; iterListPoints < pointsPolyg.Count - 1; iterListPoints++) // for points all line
            {
                if (cancellationToken.IsCancellationRequested) // cancel task
                {
                    EndCalcPoints = true;
                    cancellationToken.ThrowIfCancellationRequested();
                }

                Point point1 = pointsPolyg[iterListPoints];
                Point point2 = pointsPolyg[iterListPoints + 1];

                int x0 = point1.X;
                int y0 = point1.Y;

                int x1 = point2.X;
                int y1 = point2.Y;

                // calc points
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
                    if (cancellationToken.IsCancellationRequested) // cancel task
                    {
                        EndCalcPoints = true;
                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    // add dote at points calc
                    pointsCalculating.Enqueue(new Point(steep ? y : x, steep ? x : y)); // Не забываем вернуть координаты на место

                    if (pointsCalculating.Count >= Min_Accumulating_Points) // accumulate 10 dotes add them to user
                    {
                        lock (block)
                        {
                            for (int i = 0; i < Min_Accumulating_Points; i++)
                            {
                                pointsForUser.Enqueue(pointsCalculating.Dequeue()); // points for user
                            }
                        }
                    }

                    error -= dy;
                    if (error < 0)
                    {
                        y += ystep;
                        error += dx;
                    }
                }

            }
            lock (block)
            {
                EndCalcPoints = true; // end of calculating

                for (int i = 0; i < pointsCalculating.Count; i++) // last points
                {
                    pointsForUser.Enqueue(pointsCalculating.Dequeue()); // points for user
                }
            }

        }

        void Swap(ref int t1, ref int t2)
        {
            int temp = t1;
            t1 = t2;
            t2 = temp;
        }
    }
}
