using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2.MyFigures
{
    class Polygon //: IDrawFigure
    {
        //private Stack<Point> stackPoints; 
        //private bool endDraw; // end of drawing polygon

        //private Point CurrentPoint { get; set; } // beg point end point
        //private Point TargetPoint { get; set; }

        //public bool EndDraw { get => endDraw; private set => endDraw = value; }

        //private bool steep = false;
        //private int dx = 0;
        //private int dy = 0;
        //private int error = 0;
        //private int y = 0;

        //public Polygon(Stack<Point> points)
        //{
        //    if (points == null || points.Count == 0)
        //        throw new Exception("Empty stack");

        //    stackPoints = points;
        //}

        //public void DrawFirstPoint(Graphics gr, Pen pen, SolidBrush brush)
        //{ 
        //    if (stackPoints.Count == 0) // was one point
        //    {
        //        Point point = stackPoints.Pop();
        //        gr.FillRectangle(new SolidBrush(pen.Color), (float)point.X, (float)point.Y, pen.Width, pen.Width);
        //        EndDraw = true;
        //    }
        //    else //many point
        //    {
        //        CurrentPoint = stackPoints.Pop();
        //        TargetPoint = stackPoints.Pop();
        //        ConfigurePoints(CurrentPoint, TargetPoint); // configure point
        //    }            
        //}

        //private void ConfigurePoints(Point currentPoint, Point targetPoint)
        //{
        //    throw new NotImplementedException();
        //}

        //public void DrawNextPoint(Graphics gr, Pen pen, SolidBrush brush)
        //{
            
        //}
        
    }
}
