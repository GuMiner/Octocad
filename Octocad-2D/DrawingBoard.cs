using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;

namespace Octocad_2D
{
    /// <summary>
    /// Defines a drawing board that the lines, shapes, etc are drawn on.
    /// </summary>
    public class DrawingBoard
    {
        private List<Primitive> primitives;
        private List<Knot> knots;
        private List<CalculatedEndpoint> calculatedEndpoints;

        private Line creationLine;
        private Arc creationArc;
        private int creationStage;
        private Knot creationKnotPrimary, creationKnotSecondary; // At start of object creation and at end of object creation.
        private int creationKnotIdxPrimary, creationKnotIdxSecondary;
        private bool incrementedStage;
        private bool isSnappedToEndpoint;
        private bool isSnappedToPrimitive;
        private int snapIdx, primitiveIdx;

        SolidBrush drawBackgroundBrush;
        Pen gridPen, stencilPen, specialPen, activePen; // Should be in increasing orders of darkness

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
            knots = new List<Knot>();
            calculatedEndpoints = new List<CalculatedEndpoint>();

            creationLine = null;
            creationArc = null;
            creationKnotPrimary = null;
            creationKnotSecondary = null;
            creationKnotIdxPrimary = -1;
            creationKnotIdxSecondary = -1;
            creationStage = 0;
            incrementedStage = false;
            isSnappedToEndpoint = false;
            isSnappedToPrimitive = false;
            snapIdx = -1;
            primitiveIdx = -1;

            drawBackgroundBrush = new SolidBrush(Color.Cornsilk);
            gridPen = new Pen(Color.LightGray, 1);
            stencilPen = new Pen(Color.DarkGray, 2);
            specialPen = new Pen(Color.DarkOrange, 2);
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
                    creationLine.UpdatePrimitive();
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
                        creationArc.UpdatePrimitive();
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
                        if (creationArc.thetaTwo < creationArc.thetaOne)
                        {
                            creationArc.thetaTwo += 2 * Math.PI;
                        }

                        // Allow for full circles
                        if (Math.Abs(creationArc.thetaTwo - (creationArc.thetaOne + 2*Math.PI)) < Arc.UNIFIED_ARC)
                        {
                            creationArc.thetaTwo = creationArc.thetaOne + 2 * Math.PI;
                        }
                        creationArc.UpdatePrimitive();
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
                isSnappedToEndpoint = false;
                isSnappedToPrimitive = false;

                // Actual endpoints take precedence over calculated endpoints
                for (int i = 0; i < calculatedEndpoints.Count; i++)
                {
                    if (Utility.VisualDistance(calculatedEndpoints[i].endpointPoint, x, y) < Preferences.GetScreenErrorResolution())
                    {
                        mouseX = calculatedEndpoints[i].endpointPoint.X;
                        mouseY = calculatedEndpoints[i].endpointPoint.Y;
                        isSnappedToEndpoint = true;
                        snapIdx = i;
                        break;
                    }
                }

                for (int i = 0; i < primitives.Count; i++)
                {
                    for (int j = 0; j < primitives[i].GetEndpoints().Count; j++)
                    {
                        PointF endpoint = primitives[i].GetEndpoints()[j];
                        if (Utility.VisualDistance(endpoint, x, y) < Preferences.GetScreenErrorResolution())
                        {
                            mouseX = endpoint.X;
                            mouseY = endpoint.Y;
                            isSnappedToPrimitive = true;
                            snapIdx = j;
                            primitiveIdx = i;
                            break;
                        }
                    }

                    if (isSnappedToPrimitive)
                    {
                        break;
                    }
                }
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
                    creationLine = new Line() { x1 = mouseX, y1 = mouseY , x2 = mouseX, y2 = mouseY };
                    break;
                case Toolbox.EditMode.ARC:
                    creationArc = new Arc() { xc = mouseX, yc = mouseY, radius = 0, thetaOne = 0, thetaTwo = 0 };
                    creationStage = 0;
                    incrementedStage = false;
                    break;
                default:
                    break;
            }

            if (isSnappedToEndpoint)
            {
                //int snappedKnot = FindSnappedKnotByIntersection(
            }

            if (isSnappedToPrimitive)
            {
                // TODO
            }
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
                        GenerateCalculatedEndpoints();
                    }
                    break;
                case Toolbox.EditMode.ARC:
                    if (creationArc != null)
                    {
                        primitives.Add(creationArc);
                        creationArc = null;
                        GenerateCalculatedEndpoints();
                    }
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

        public void GenerateCalculatedEndpoints()
        {
            calculatedEndpoints = new List<CalculatedEndpoint>();
            for (int i = 0; i < primitives.Count; i++)
            {
                for (int j = i; j < primitives.Count; j++)
                {
                    // Determine where this primitive intersects the other primitive -- if there is an intersection, add that to the calculated endpoint list.
                    List<PointF> intersections = Utility.GetPrimitiveIntersections(primitives[i], primitives[j]);
                    foreach (PointF point in intersections)
                    {
                        calculatedEndpoints.Add(new CalculatedEndpoint() { endpointPoint = point, firstPrimitive = i, secondPrimitive = j });
                    }
                }
            }
        }

        /// <summary>
        /// Starts the extrude process. In this process, the user selects what is to be extruded.
        /// </summary>
        public void ExtrudeDrawing()
        {
            // Step 1: Normalize lines to all terminate on intersection or normal endpoints.

            // Step 2: Draw onto a life-sized bitmap.
            Bitmap flatMapping = new Bitmap((int)(Preferences.length/Preferences.resolution), (int)(Preferences.height/Preferences.resolution), PixelFormat.Format1bppIndexed);
            DrawForConversion(ref flatMapping);

            // Step 3: Show to user, let user select segments for extrusion / revolve point.
            BitSelectionPane bsp = new BitSelectionPane(ref flatMapping);
            bsp.ShowDialog(); // Hold until the user hits cancel or ok
            if (bsp.okToProceed)
            {

            }

            // Step 4: Send filled-out bitmap to Octocad C++ for extrusion / revolving.
        }

        /// <summary>
        /// Draws the bit image of all the items to form the potential extrusion/rotate map.
        /// </summary>
        /// <param name="bitImage"></param>
        public void DrawForConversion(ref Bitmap bitImage)
        {
            BitmapData bitmapData = bitImage.LockBits(new Rectangle(0, 0, bitImage.Width, bitImage.Height), ImageLockMode.ReadWrite, bitImage.PixelFormat);

            // Clear the background
            for (int y = 0; y < bitmapData.Height; y++)
            {
                for (int x = 0; x < bitmapData.Width; x++)
                {
                    ClearPixel(x, y, ref bitmapData);
                }
            }

            // Draw the primitives
            foreach (Primitive primitive in primitives)
            {
                primitive.DrawForExport(ref bitmapData);
            }

            bitImage.UnlockBits(bitmapData);

            bitImage.Save("C:\\users\\gustave\\desktop\\result.png");
        }

        /// <summary>
        /// Verifies the point is within the pixel bitmap.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="bitmapData"></param>
        /// <returns></returns>
        public static bool VerifyBounds(int x, int y, ref BitmapData bitmapData)
        {
            return (x >= 0 && y >= 0 && x < bitmapData.Width && y < bitmapData.Height);
        }

        /// <summary>
        /// Clears the pixel at the provided location.
        /// This actually sets the bit (which is clear).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="bitmapData"></param>
        public static void ClearPixel(int x, int y, ref BitmapData bitmapData)
        {
            unsafe
            {
                byte* row = (byte*)bitmapData.Scan0 + (y * bitmapData.Stride) + (x / 8);
                switch (x % 8)
                {
                    case 0:
                        *row = (byte)(*row | 0x80);
                        break;
                    case 1:
                        *row = (byte)(*row | 0x40);
                        break;
                    case 2:
                        *row = (byte)(*row | 0x20);
                        break;
                    case 3:
                        *row = (byte)(*row | 0x10);
                        break;
                    case 4:
                        *row = (byte)(*row | 0x08);
                        break;
                    case 5:
                        *row = (byte)(*row | 0x04);
                        break;
                    case 6:
                        *row = (byte)(*row | 0x02);
                        break;
                    case 7:
                        *row = (byte)(*row | 0x01);
                        break;
                }
            }
        }

        /// <summary>
        /// Sets the pixel at the specified location, in an unsafe manner.
        /// This actually clears the bit (which is solid).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="bitmapData"></param>
        public static void SetPixel(int x, int y, ref BitmapData bitmapData)
        {
            unsafe
            {
                byte* row = (byte*)bitmapData.Scan0 + (y * bitmapData.Stride) + (x / 8);
                switch (x % 8)
                {
                    case 0:
                        *row = (byte)(*row & 0x7F);
                        break;
                    case 1:
                        *row = (byte)(*row & 0xBF);
                        break;
                    case 2:
                        *row = (byte)(*row & 0xDF);
                        break;
                    case 3:
                        *row = (byte)(*row & 0xEF);
                        break;
                    case 4:
                        *row = (byte)(*row & 0xF7);
                        break;
                    case 5:
                        *row = (byte)(*row & 0xFB);
                        break;
                    case 6:
                        *row = (byte)(*row & 0xFD);
                        break;
                    case 7:
                        *row = (byte)(*row & 0xFE);
                        break;
                }
            }
        }

        /// <summary>
        /// Checks if the given pixel on the image is set or not.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="bitmapData"></param>
        /// <returns>True if set, false otherwise</returns>
        public static bool CheckPixel(int x, int y, ref BitmapData bitmapData)
        {
            unsafe
            {
                byte* row = (byte*)bitmapData.Scan0 + (y * bitmapData.Stride) + (x / 8);
                switch (x % 8)
                {
                    case 0:
                        return (byte)(*row & 0x80) == 0;
                    case 1:
                        return (byte)(*row & 0x40) == 0;
                    case 2:
                        return (byte)(*row & 0x20) == 0;
                    case 3:
                        return (byte)(*row & 0x10) == 0;
                    case 4:
                        return (byte)(*row & 0x08) == 0;
                    case 5:
                        return (byte)(*row & 0x04) == 0;
                    case 6:
                        return (byte)(*row & 0x02) == 0;
                    case 7:
                        return (byte)(*row & 0x01) == 0;
                }
            }
            return true;
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

            // Draw the calculated endpoints
            foreach (CalculatedEndpoint endpoint in calculatedEndpoints)
            {
                double eX, eY;
                DrawingBoard.MapToScreen(endpoint.endpointPoint.X, endpoint.endpointPoint.Y, out eX, out eY);
                g.DrawRectangle(specialPen, (float)eX - 2, (float)eY - 2, 4, 4);
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