using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Octocad_2D
{
    public partial class BitSelectionPane : Form
    {
        public bool okToProceed;

        private Bitmap originalBitmapCopy;
        public Bitmap edittedBitmap;
        public double distance;
        public bool isDistanceMirrored;

        public BitSelectionPane(ref Bitmap originalBitmap)
        {
            InitializeComponent();
            unitLabel.Text = Preferences.units;

            originalBitmapCopy = originalBitmap;
            resetButton_Click(null, null);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            okToProceed = false;
            Hide();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            okToProceed = true;
            isDistanceMirrored = isMirrored.Checked;
            distance = Double.Parse(distanceBox.Text);
            Hide();
        }

        private void bitmapPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(originalBitmapCopy, 0, 0, bitmapPanel.Width, bitmapPanel.Height);
            e.Graphics.DrawImage(edittedBitmap, 0, 0, bitmapPanel.Width, bitmapPanel.Height);
        }

        private void BitSelectionPane_Resize(object sender, EventArgs e)
        {
            bitmapPanel.Invalidate();
        }


        private void bitmapPanel_MouseClick(object sender, MouseEventArgs e)
        {
            // Find out where the user clicked on the image.
            int xP = (int)((double)e.X * originalBitmapCopy.Width / bitmapPanel.Width);
            int yP = (int)((double)e.Y * originalBitmapCopy.Height / bitmapPanel.Height);
            if (xP >= 0 && yP >= 0 && xP < originalBitmapCopy.Width && yP < originalBitmapCopy.Height)
            {
                FillOrClear(xP, yP);
            }

            bitmapPanel.Invalidate();
        }

        /// <summary>
        /// Fills or clears the mask bitmap (edittedBitmap) based upon the restriction bitmap (originalBitmapCopy)
        /// </summary>
        /// <param name="xP"></param>
        /// <param name="yP"></param>
        private void FillOrClear(int xP, int yP)
        {
            BitmapData restrictionBitmap = originalBitmapCopy.LockBits(new Rectangle(0, 0, originalBitmapCopy.Width, originalBitmapCopy.Height), ImageLockMode.ReadWrite, originalBitmapCopy.PixelFormat);
            BitmapData bitmapData = edittedBitmap.LockBits(new Rectangle(0, 0, edittedBitmap.Width, edittedBitmap.Height), ImageLockMode.ReadWrite, edittedBitmap.PixelFormat);

            // We're trying to get points into this state.
            bool isSetFilled = !DrawingBoard.CheckPixel(xP, yP, ref bitmapData);

            Stack<Vector2i> pointsToModify = new Stack<Vector2i>();
            pointsToModify.Push(new Vector2i(xP, yP));

            while (pointsToModify.Count != 0)
            {
                Vector2i point = pointsToModify.Pop();
                if (isSetFilled)
                {
                    DrawingBoard.SetPixel(point.x, point.y, ref bitmapData);
                }
                else
                {
                    DrawingBoard.ClearPixel(point.x, point.y, ref bitmapData);
                } 

                // Check left, right, up, and down.
                if (CheckAddPoint(point.x + 1, point.y, isSetFilled, ref bitmapData, ref restrictionBitmap))
                {
                    pointsToModify.Push(new Vector2i(point.x + 1, point.y));
                }
                if (CheckAddPoint(point.x, point.y + 1, isSetFilled, ref bitmapData, ref restrictionBitmap))
                {
                    pointsToModify.Push(new Vector2i(point.x, point.y + 1));
                }
                if (CheckAddPoint(point.x - 1, point.y, isSetFilled, ref bitmapData, ref restrictionBitmap))
                {
                    pointsToModify.Push(new Vector2i(point.x - 1, point.y));
                }
                if (CheckAddPoint(point.x, point.y - 1, isSetFilled, ref bitmapData, ref restrictionBitmap))
                {
                    pointsToModify.Push(new Vector2i(point.x, point.y - 1));
                }
            }

            edittedBitmap.UnlockBits(bitmapData);
            originalBitmapCopy.UnlockBits(restrictionBitmap);
        }

        private bool CheckAddPoint(int x, int y, bool isSetFilled, ref BitmapData bitmapData, ref BitmapData restrictionBitmapData)
        {
            if (!DrawingBoard.VerifyBounds(x, y, ref bitmapData))
            {
                return false;
            }

            bool isFilled = DrawingBoard.CheckPixel(x, y, ref bitmapData);
            if (isSetFilled == isFilled)
            {
                return false;
            }

            isFilled = DrawingBoard.CheckPixel(x, y, ref restrictionBitmapData);
            if (isFilled)
            {
                return false;
            }

            return true;
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            // Clear the mask bitmap
            edittedBitmap = new Bitmap(originalBitmapCopy.Width, originalBitmapCopy.Height, originalBitmapCopy.PixelFormat);
            BitmapData bitmapData = edittedBitmap.LockBits(new Rectangle(0, 0, edittedBitmap.Width, edittedBitmap.Height), ImageLockMode.ReadWrite, edittedBitmap.PixelFormat);

            for (int i = 0; i < edittedBitmap.Width; i++)
            {
                for (int j = 0; j < edittedBitmap.Height; j++)
                {
                    DrawingBoard.ClearPixel(i, j, ref bitmapData);
                }
            }

            edittedBitmap.UnlockBits(bitmapData);

            // Set the edit palette so that white is actually transparent, so we can see the background
            ColorPalette palette = edittedBitmap.Palette;
            palette.Entries[1] = Color.FromArgb(0, 255, 255, 255); // Clear
            palette.Entries[0] = Color.FromArgb(255, 0, 255, 0); // Solid
            edittedBitmap.Palette = palette;

            bitmapPanel.Invalidate();
        }

        private void distanceBox_TextChanged(object sender, EventArgs e)
        {
            double dist;
            bool parseSuccessful = Double.TryParse(distanceBox.Text, out dist);
            if (!parseSuccessful)
            {
                MessageBox.Show("Could not parse the distance as a valid number!");
                distanceBox.Text = "1.0";
            }
        }
    }
}
