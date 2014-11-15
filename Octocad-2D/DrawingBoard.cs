using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Octocad_2D
{
    /// <summary>
    /// Defines a drawing board that the lines, shapes, etc are drawn on.
    /// </summary>
    class DrawingBoard
    {
        private List<Primitive> primitives;
        private List<Knot> knots;
        private Line creationLine;
        private Arc creationArc;
        private int creationStage;
        private bool incrementedStage;

        SolidBrush drawBackgroundBrush;
        Pen gridPen, stencilPen, activePen; // Should be in increasing orders of darkness

        /// <summary>
        /// The actual size of the drawing board
        /// </summary>
        private static double xMin, xMax, yMin, yMax;

        /// <summary>
        /// The percentage of the screen (x-direction) that is displayed, and the x and y offsets (in centralized screen coordinates)
        /// Also, the counters for delta motion when the screen is dragged
        /// </summary>
        private static double xDrawPercentage, xOffset, yOffset, mx, my;

        /// <summary>
        /// In board coordinates
        /// </summary>
        private double mouseX, mouseY;

        public DrawingBoard()
        {
            primitives = new List<Primitive>();
            creationLine = null;
            creationArc = null;

            creationStage = 0;
            incrementedStage = false;

            drawBackgroundBrush = new SolidBrush(Color.Cornsilk);
            gridPen = new Pen(Color.LightGray, 1);
            stencilPen = new Pen(Color.DarkGray, 2);
            activePen = new Pen(Color.DarkGreen, 2);

            UpdateFromPreferences();
            mouseX = 0;
            mouseY = 0;
        }

        public void UpdateFromPreferences()
        {
            xMin = -Preferences.length/2;
            xMax = Preferences.length/2;
            yMin = -Preferences.width/2;
            yMax = Preferences.width/2;
            xDrawPercentage = 1;
            xOffset = 0;
            yOffset = 0;
        }

        /// <summary>
        /// Maps a point from the board to the screen.
        /// </summary>
        /// <param name="xP"></param>
        /// <param name="yP"></param>
        /// <param name="xSP"></param>
        /// <param name="ySP"></param>
        public static void MapToScreen(double xP, double yP, out double xSP, out double ySP)
        {
            double xPercent = (xP - xMin) / (xMax - xMin);
            double yPercent = (yP - yMin) / (xMax - xMin);

            double screenDifference = Octocad2D.EWidth * xDrawPercentage - Octocad2D.EWidth;
            xSP = xPercent * Octocad2D.EWidth * xDrawPercentage - screenDifference / 2 + xOffset;
            ySP = yPercent * Octocad2D.EWidth * xDrawPercentage - screenDifference / 2 + yOffset;
        }

        /// <summary>
        /// Maps a point from the screen to the board
        /// </summary>
        /// <param name="xSP"></param>
        /// <param name="ySP"></param>
        /// <param name="xP"></param>
        /// <param name="yP"></param>
        public static void MapToBoard(double xSP, double ySP, out double xP, out double yP)
        {
            double screenDifference = Octocad2D.EWidth * xDrawPercentage - Octocad2D.EWidth;

            double xPercent = (xSP + screenDifference / 2 - xOffset) / (xDrawPercentage * Octocad2D.EWidth);
            double yPercent = (ySP + screenDifference / 2 - yOffset) / (xDrawPercentage * Octocad2D.EWidth);

            xP = (xMax - xMin) * xPercent + xMin;
            yP = (xMax - xMin) * yPercent + yMin;
        }

        public void HandleMouseMotion(int x, int y)
        {
            SnapHandleMouse(x, y);
        }

        public void HandleMouseDrag(int x, int y)
        {
            if (Toolbox.editMode == Toolbox.EditMode.SELECT)
            {
                double deltaX = x - mx;
                double deltaY = y - my;
                xOffset += deltaX;
                yOffset += deltaY;
            }
            else if (Toolbox.editMode == Toolbox.EditMode.LINE)
            {
                if (creationLine != null)
                {
                    creationLine.x2 = mouseX;
                    creationLine.y2 = mouseY;
                }
            }
            else if (Toolbox.editMode == Toolbox.EditMode.ARC)
            {
                if (creationArc != null)
                {
                    if (creationStage == 0)
                    {
                        // Determining the radius and initial angle
                        double mouseXDiff = (mouseX - creationArc.xc);
                        double mouseYDiff = (mouseY - creationArc.yc);
                        creationArc.radius = Math.Sqrt(mouseXDiff*mouseXDiff + mouseYDiff*mouseYDiff);
                        creationArc.thetaOne = Math.Atan2(mouseYDiff, mouseXDiff);
                        if (creationArc.thetaOne < 0)
                        {
                            creationArc.thetaOne += 2 * Math.PI;
                        }
                    }
                    else
                    {
                        // Determining the swept out area.
                        double mouseXDiff = (mouseX - creationArc.xc);
                        double mouseYDiff = (mouseY - creationArc.yc);
                        creationArc.thetaTwo = Math.Atan2(mouseYDiff, mouseXDiff);
                        if (creationArc.thetaTwo < 0)
                        {
                            creationArc.thetaTwo += 2 * Math.PI;
                        }
                    }
                }
            }

            SnapHandleMouse(x, y);
        }

        /// <summary>
        /// Snaps the mouse to the grid (or nearby endpoints), if applicable, and updates the mouse motion delta trackers
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void SnapHandleMouse(int x, int y)
        {
            // Snap the mouse to the grid (or just pass through)
            double xB, yB;
            DrawingBoard.MapToBoard(x, y, out xB, out yB);

            if (Toolbox.isSnapToGrid)
            {
                mouseX = (int)((xB + 5) / 10) * 10;
                mouseY = (int)((yB + 5) / 10) * 10;
            }
            else
            {
                mouseX = xB;
                mouseY = yB;
            }

            // Takes precedent over grid snapping, so calculated after a potential grid snap.
            if (Toolbox.isSnapEndpoints)
            {
            }

            // Update mouse motion delta
            mx = x;
            my = y;
        }

        public void HandleMouseScroll(int ticks)
        {
            if (ticks > 0)
            {
                xDrawPercentage *= 1.125;
            }
            else if (ticks < 0)
            {
                xDrawPercentage *= 0.88;
            }
        }

        /// <summary>
        /// Early exit of many actions
        /// </summary>
        public void HandleEscape()
        {
            switch (Toolbox.editMode)
            {
                case Toolbox.EditMode.LINE:
                    creationLine = null;
                    break;
                case Toolbox.EditMode.ARC:
                    creationArc = null;
                    break;
                default:
                    break;
            }
        }

        public void LeftDown()
        {
            switch(Toolbox.editMode)
            {
                case Toolbox.EditMode.LINE:
                    creationLine = new Line() { x1 = mouseX, y1 = mouseY };
                    break;
                case Toolbox.EditMode.ARC:
                    creationArc = new Arc() { xc = mouseX, yc = mouseY };
                    creationStage = 0;
                    incrementedStage = false;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Some tools need an additional verifier input to procede to the next stage.
        /// </summary>
        public void RightDown()
        {
            if (!incrementedStage)
            {
                ++creationStage;
            }
            incrementedStage = true;
        }

        public void RightUp()
        {
            incrementedStage = false;
        }

        public void LeftUp()
        {
            switch (Toolbox.editMode)
            {
                case Toolbox.EditMode.LINE:
                    if (creationLine != null)
                    {
                        primitives.Add(creationLine);
                        creationLine = null;
                    }
                    break;
                case Toolbox.EditMode.ARC:
                    if (creationArc != null)
                    {
                        primitives.Add(creationArc);
                        creationArc = null;
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Snaps the provided point to the screen
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void SnapToScreen(ref double x, ref double y)
        {
            if (x < 0)
            {
                x = 0;
            }
            else if (x > Octocad2D.EWidth)
            {
                x = Octocad2D.EWidth;
            }

            if (y < 0)
            {
                y = 0;
            }
            else if (y > Octocad2D.EHeight)
            {
                y = Octocad2D.EHeight;
            }
        }

        public void Draw(Graphics g)
        {
            // Draw the drawing background
            double xL, yL, xH, yH;
            MapToScreen(xMin, yMin, out xL, out yL);
            MapToScreen(xMax, yMax, out xH, out yH);
            SnapToScreen(ref xL, ref yL);
            SnapToScreen(ref xH, ref yH);
            g.FillRectangle(drawBackgroundBrush, (float)xL, (float)yL, (float)xH - (float)xL, (float)yH - (float)yL);

            // Draw a really basic grid for now
            for (int i = -60; i <= 60; i+=10)
            {
                double x1 = i;
                double y1 = yMin;
                double x2 = i;
                double y2 = yMax;
                double xt, yt, xxt, yyt;
                MapToScreen(x1, y1, out xt, out yt);
                MapToScreen(x2, y2, out xxt, out yyt);
                g.DrawLine(gridPen, (float)xt, (float)yt, (float)xxt, (float)yyt);
            }
            for (int i = -50; i <= 50; i+=10)
            {
                double x1 = xMin;
                double y1 = i;
                double x2 = xMax;
                double y2 = i;
                double xt, yt, xxt, yyt;
                MapToScreen(x1, y1, out xt, out yt);
                MapToScreen(x2, y2, out xxt, out yyt);
                g.DrawLine(gridPen, (float)xt, (float)yt, (float)xxt, (float)yyt);
            }

            // Draw the current pointer
            double mouseXX, mouseYY;
            MapToScreen(mouseX, mouseY, out mouseXX, out mouseYY);
            g.DrawEllipse(stencilPen, (float)mouseXX - 5, (float)mouseYY - 5, 10, 10);
            g.DrawString(mouseX + " " + mouseY + " " + mouseXX + " " + mouseYY, new Font("Arial", 12), new SolidBrush(Color.Green), 0, 0);

            // Draw the primitives
            foreach (Primitive primitive in primitives)
            {
                primitive.Draw(g, stencilPen);
            }

            // Draw the line being created
            if (creationLine != null)
            {
                creationLine.Draw(g, activePen);
            }

            // Draw the arc being created
            if (creationArc != null)
            {
                creationArc.Draw(g, activePen);
                if (creationStage == 0)
                {
                    // Also draw a line from the center to the current mouse position (for radius determination).
                    double xc, yc;
                    MapToScreen(creationArc.xc, creationArc.yc, out xc, out yc);
                    g.DrawLine(activePen, (float)xc, (float)yc, (float)mouseXX, (float)mouseYY);
                }
            }
        }
    }
}