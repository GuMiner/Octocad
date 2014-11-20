using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Octocad_2D
{
    public partial class Octocad2D : Form
    {
        Bitmap drawingBitmap;
        SolidBrush backgroundBrush = new SolidBrush(Color.White);

        private static Toolbox toolbox;
        private static Preferences preferences;
        private static DrawingBoard drawingBoard;

        private ProcessLink processLink;

        // If the file has been saved since an edit
        private bool hasSaved;

        // If the file hasn't been edited, ever.
        private bool isNew;

        public static int EWidth;
        public static int EHeight;

        private bool convertToDrag = false;

        public Octocad2D()
        {
            InitializeComponent();
            MouseWheel += Octocad2D_MouseWheel;

            processLink = new ProcessLink();
            preferences = new Preferences(processLink);
            drawingBoard = new DrawingBoard(processLink);

            toolbox = new Toolbox(drawingBoard);
            toolbox.Show();
            
            

            hasSaved = false;
            isNew = true;
            saveAsToolStripMenuItem.Enabled = false;

            Octocad2D_Resize(null, null);
        }

        /// <summary>
        /// Renders the 2D drawing area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Octocad2D_Paint(object sender, PaintEventArgs e)
        {
            // Update any controls, like the taskbar
            foreach (Control ctrl in Controls)
            {
                ctrl.Refresh();
            } 

            // Draw the main screen
            DrawDrawingArea(e.Graphics);
        }

        private void RecreateBitmap()
        {
            drawingBitmap = new Bitmap(EWidth, EHeight);
            this.Invalidate(true);
        }

        private void DrawDrawingArea(Graphics g)
        {
            // Clear the screen
            Graphics gg = Graphics.FromImage(drawingBitmap);
            gg.FillRectangle(backgroundBrush, 0, 0, EWidth, EHeight);

            // Draw the current active items
            drawingBoard.Draw(gg);

            // Copy the bitmap onto the drawing screen.
            g.DrawImage(drawingBitmap, 0, topMenuStrip.Height);
            gg.Dispose();
        }

        private void Octocad2D_Resize(object sender, EventArgs e)
        {
            EWidth = ClientSize.Width;
            EHeight = ClientSize.Height - topMenuStrip.Height;
            RecreateBitmap();
        }

        private void MarkEdit()
        {
            hasSaved = false;
            isNew = false;
            
        }

        private void MarkSaved()
        {
            hasSaved = true;
            saveAsToolStripMenuItem.Enabled = true;
        }

        
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Attempts a direct exit, which combines both exit schemas.
            Application.Exit();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MarkEdit();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO do saving


            MarkSaved();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            MarkSaved();
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            preferences.Show();
            // TODO perform actions based on updates from preferences.
        }

        /// <summary>
        /// Shows the help pane.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new Help().Show();
        }

        /// <summary>
        /// Shows the about pane.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().Show();
        }

        /// <summary>
        /// Closes the form after first verifying that the user understands the condition of the save.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Octocad2D_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!hasSaved && !isNew)
            {
                DialogResult result = MessageBox.Show("Do you want to save?", "Unsaved Edits Detected", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                    if (!hasSaved)
                    {
                        e.Cancel = true; // Didn't successfully save
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true; // Don't exit at all.
                }
            }
        }

        private void Octocad2D_MouseMove(object sender, MouseEventArgs e)
        {
            if (convertToDrag)
            {
                drawingBoard.HandleMouseDrag(e.X, e.Y - topMenuStrip.Height);
            }
            else
            {
                drawingBoard.HandleMouseMotion(e.X, e.Y - topMenuStrip.Height);
            }
            this.Invalidate(true);
        }

        void Octocad2D_MouseWheel(object sender, MouseEventArgs e)
        {
            drawingBoard.HandleMouseScroll(e.Delta);
            this.Invalidate(true);
        }

        private void Octocad2D_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    convertToDrag = true;
                    drawingBoard.LeftDown();
                    break;
                case MouseButtons.Right:
                    drawingBoard.RightDown();
                    break;
                default:
                    break;
            }
            this.Invalidate(true);
        }

        private void Octocad2D_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    convertToDrag = false;
                    drawingBoard.LeftUp();
                    break;
                case MouseButtons.Right:
                    drawingBoard.RightUp();
                    break;
                default:
                    break;
            }
            this.Invalidate(true);
        }

        private void Octocad2D_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    drawingBoard.HandleEscape();
                    this.Invalidate(true);
                    break;
                default:
                    break;
            }
        }
    }
}
