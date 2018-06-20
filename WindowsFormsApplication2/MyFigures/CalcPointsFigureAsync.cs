using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2.MyFigures
{
    class CalcPointsFigureAsync
    {
        private const int Min_Accumulating_Points = 10;
        private readonly object block = new object(); // blocker

        private bool endCalcPoints = false;
        

        private Queue<Point> pointsForUser = new Queue<Point>();

        private Queue<Point> pointsCalculating = new Queue<Point>();

        public bool EndCalcPoints
        {
            get
            {
                lock(block)
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

        public async void CalcPolygon(List<Point> points)
        {
            EndCalcPoints = false; // not end of calc
            await Task.Factory.StartNew(CalcPolygonPoints, points);
        }

        // get points accumulated quantity
        public Queue<Point> TryGetAccumulatedPoints()
        {            
            lock(block)
            {           
                if (pointsForUser.Count >= Min_Accumulating_Points)
                {
                    Queue<Point> retPoints = new Queue<Point>();
                    if (pointsForUser.Count != 0)
                    {
                        for(int i=0; i<Min_Accumulating_Points; i++) // accumulated points in result and return
                            retPoints.Enqueue(pointsForUser.Dequeue()); 
                    }
                    return retPoints;
                }
            }
            return null;
        }

        void CalcPolygonPoints(object points)
        {
            List<Point> pointsPolyg = points as List<Point>;

            for (int iterListPoints = 0; iterListPoints < pointsPolyg.Count - 1; iterListPoints++) // for points all line
            {
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
