using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Octocad_2D
{
    class Line : Primitive
    {
        // Points defining the line.
        public double x1, y1, x2, y2;
        public List<PointF> endpoints;

        public Line()
        {
            endpoints = new List<PointF>();
        }

        public void Draw(Graphics g, Pen pen)
        {
            double xs1, ys1, xs2, ys2;
            DrawingBoard.MapToScreen(x1, y1, out xs1, out ys1);
            DrawingBoard.MapToScreen(x2, y2, out xs2, out ys2);
            g.DrawLine(pen, (float)xs1, (float)ys1, (float)xs2, (float)ys2);

            DrawEndpoints(g, pen);
        }

        public void DrawForExport(ref BitmapData bitmapData)
        {
            double distance = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
            double steps = 2*distance / (Preferences.resolution);

            // Drawing the line in a parametric manner.
            double x_s = x1 + Preferences.length/2;
            double y_s = y1 + Preferences.length/2;
            for (double i = 0; i < steps; i++)
            {
                double x_n = ((i/steps)*(x2 - x1) + x_s)/Preferences.resolution;
                double y_n = ((i/steps)*(y2 - y1) + y_s)/Preferences.resolution;
                if (DrawingBoard.VerifyBounds((int)x_n, (int)y_n, ref bitmapData))
                {
                    DrawingBoard.SetPixel((int)x_n, (int)y_n, ref bitmapData);
                }
            }
        }

        public void UpdatePrimitive()
        {
            endpoints = GetEndpoints();
        }

        public void DrawEndpoints(Graphics g, Pen pen)
        {
            foreach (PointF endpoint in endpoints)
            {
                double eX, eY;
                DrawingBoard.MapToScreen(endpoint.X, endpoint.Y, out eX, out eY);
                g.DrawRectangle(pen, (float)eX - 2, (float)eY - 2, 4, 4);
            }
        }

        public void DrawSelectedEndpoint(int endpoint, Graphics g, Pen pen)
        {

        }

        public List<PointF> GetEndpoints()
        {
            List<PointF> returnList = new List<PointF>();
            returnList.Add(new PointF((float)x1, (float)y1));
            returnList.Add(new PointF((float)x2, (float)y2));

            return returnList;
        }


        public Primitive Copy()
        {
            return new Line() { x1 = this.x1, y1 = this.y1, x2 = this.x2, y2 = this.y2 };
        }
    }
}
