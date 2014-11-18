using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Octocad_2D
{
    class Utility
    {
        public static List<PointF> GetPrimitiveIntersections(Primitive first, Primitive second)
        {
            if (first is Line && second is Line)
            {
                return GetLineLineIntersections((Line)first, (Line)second);
            }
            else if (first is Line && second is Arc)
            {
                return GetLineArcIntersections((Arc)second, (Line)first);
            }
            else if (first is Arc && second is Line)
            {
                return GetLineArcIntersections((Arc)first, (Line)second);
            }
            else if (first is Arc && second is Arc)
            {
                return GetArcArcIntersections((Arc)first, (Arc)second);
            }
            return new List<PointF>();
        }

        private static List<PointF> GetLineLineIntersections(Line first, Line second)
        {
            return new List<PointF>();
        }

        private static List<PointF> GetLineArcIntersections(Arc first, Line second)
        {
            return new List<PointF>();
        }

        private static List<PointF> GetArcArcIntersections(Arc first, Arc second)
        {
            return new List<PointF>();
        }

        /// <summary>
        /// Distance of points as seen on screen.
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double VisualDistance(PointF firstPoint, int x, int y)
        {
            double xS, yS;
            DrawingBoard.MapToScreen(firstPoint.X, firstPoint.Y, out xS, out yS);

            return Math.Sqrt((x - xS) * (x - xS) + (y - yS) * (y - yS));
        }
    }
}
