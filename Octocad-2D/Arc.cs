using System;
using System.Drawing;

namespace Octocad_2D
{
    /// <summary>
    /// TODO make the arc not only do circle segments.
    /// </summary>
    class Arc : Primitive
    {
        // Points defining the arc, note that arcs are CW from the x-axis and in radians
        public double xc, yc, radius, thetaOne, thetaTwo;

        public void Draw(Graphics g, Pen pen)
        {
            double xs1, ys1, xs2, ys2, xsc, ysc;
            DrawingBoard.MapToScreen(xc - radius, yc - radius, out xs1, out ys1);
            DrawingBoard.MapToScreen(xc + radius, yc + radius, out xs2, out ys2);
            DrawingBoard.MapToScreen(xc, yc, out xsc, out ysc);

            double width = xs2 - xs1;
            double height = ys2 - ys1;
            g.DrawEllipse(pen, (float)xsc - 3, (float)ysc - 3, 6, 6);
            if (radius > 0.0001) // TODO need an error factor
            {
                g.DrawArc(pen, (float)xs1, (float)ys1, (float)width, (float)height, (float)thetaOne * 180.0f / (float)Math.PI, (float)thetaTwo * 180.0f / (float)Math.PI);
            }
        }

        public Primitive Copy()
        {
            return null;
        }
    }
}
