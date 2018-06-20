using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2.MyFigures
{
    interface IDrawFigure
    {
        bool EndDraw();
        void DrawFirstPoint(Graphics gr, Pen pen, SolidBrush brush);
        void DrawNextPoint(Graphics gr, Pen pen, SolidBrush brush);
    }
}
