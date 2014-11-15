using System;
using System.Collections.Generic;
using System.Drawing;

namespace Octocad_2D
{
    class Line : Primitive
    {
        // Points defining the line.
        public double x1, y1, x2, y2;

        public void Draw(Graphics g, Pen pen)
        {
            double xs1, ys1, xs2, ys2;
            DrawingBoard.MapToScreen(x1, y1, out xs1, out ys1);
            DrawingBoard.MapToScreen(x2, y2, out xs2, out ys2);
            g.DrawLine(pen, (float)xs1, (float)ys1, (float)xs2, (float)ys2);
        }

        public Primitive Copy()
        {
            return new Line() { x1 = this.x1, y1 = this.y1, x2 = this.x2, y2 = this.y2 };
        }
    }
}
