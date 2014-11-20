using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Octocad_2D
{
    /// <summary>
    /// TODO make the arc not only do circle segments.
    /// </summary>
    class Arc : Primitive
    {
        // Points defining the arc, note that arcs are CW from the x-axis and in radians
        public double xc, yc, radius, thetaOne, thetaTwo;
        public static double UNIFIED_ARC = Math.PI / 180; // 1 degree arcs are auto-unified.

        public List<PointF> endpoints;

        public Arc()
        {
            endpoints = new List<PointF>();
        }

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
                g.DrawArc(pen, (float)xs1, (float)ys1, (float)width, (float)height, (float)thetaOne * 180.0f / (float)Math.PI, (float)(thetaTwo - thetaOne) * 180.0f / (float)Math.PI);
            }

            DrawEndpoints(g, pen);
        }

        public void DrawForExport(ref BitmapData bitmapData)
        {
            double distance = radius * Math.Abs(thetaTwo - thetaOne);
            double steps = 2 * distance / (Preferences.resolution);

            // Drawing the line in a parametric manner.
            for (double i = 0; i < steps; i++)
            {
                double x_n = (Math.Cos((i / steps) * (thetaTwo - thetaOne) + thetaOne) * radius + xc + Preferences.length / 2) / Preferences.resolution;
                double y_n = (Math.Sin((i / steps) * (thetaTwo - thetaOne) + thetaOne) * radius + yc + Preferences.length / 2) / Preferences.resolution;
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
            List<PointF> returnPoints = new List<PointF>();
            if (Math.Abs(thetaTwo - (thetaOne + Math.PI * 2)) < UNIFIED_ARC)
            {
                return returnPoints;
            }

            // Add the endpoints where the arc points are
            returnPoints.Add(new PointF((float)(xc + radius * Math.Cos(thetaOne)), (float)(yc + radius * Math.Sin(thetaOne))));
            returnPoints.Add(new PointF((float)(xc + radius * Math.Cos(thetaTwo)), (float)(yc + radius * Math.Sin(thetaTwo))));

            return returnPoints;
        }

        public Primitive Copy()
        {
            return null;
        }
    }
}
