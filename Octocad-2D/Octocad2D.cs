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

        private Toolbox toolbox;

        // If the file has been saved since an edit
        private bool hasSaved;

        // If the file hasn't been edited, ever.
        private bool isNew;

        private int EWidth
        {
            get
            {
                return ClientSize.Width;
            }
        }

        private int EHeight
        {
            get
            {
                return ClientSize.Height - topMenuStrip.Height;
            }
        }

        public Octocad2D()
        {
            InitializeComponent();
            toolbox = new Toolbox();
            toolbox.Show();

            hasSaved = false;
            isNew = true;
            saveAsToolStripMenuItem.Enabled = false;

            RecreateBitmap();
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
            Graphics gg = Graphics.FromImage(drawingBitmap);
            gg.FillRectangle(backgroundBrush, 0, 0, EWidth, EHeight);

           // HatchBrush hb = new HatchBrush(HatchStyle.SmallGrid, Color.LightBlue);
          //  gg.FillRectangle(hb, 0, 0, EWidth, EHeight);

            Pen blackPen = new Pen(Color.Black, 1);
            gg.DrawLine(blackPen, 0, 0, EWidth, EHeight);
            gg.DrawLine(blackPen, 0, EHeight, EWidth, 0);

            // Copy the bitmap onto the drawing screen.
            g.DrawImage(drawingBitmap, 0, topMenuStrip.Height);
            gg.Dispose();
        }

        private void Octocad2D_Resize(object sender, EventArgs e)
        {
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

        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

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
    }
}
