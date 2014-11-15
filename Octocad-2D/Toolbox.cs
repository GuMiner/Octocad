﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Octocad_2D
{
    public partial class Toolbox : Form
    {
        public enum EditMode { SELECT, LINE, ARC, DIMENSION };
        public static EditMode editMode;
        public static bool isSnapToGrid, isSnapEndpoints;

        public Toolbox()
        {
            InitializeComponent();
            editMode = EditMode.SELECT;
            isSnapToGrid = true;
            isSnapEndpoints = true;
        }

        private void snapToGrid_CheckedChanged(object sender, EventArgs e)
        {
            isSnapToGrid = snapToGrid.Checked;
        }

        private void snapEndpoints_CheckedChanged(object sender, EventArgs e)
        {
            isSnapEndpoints = snapEndpoints.Checked;
        }

        private void dimensionButton_CheckedChanged(object sender, EventArgs e)
        {
            editMode = EditMode.DIMENSION;
        }

        private void arcButton_CheckedChanged(object sender, EventArgs e)
        {
            editMode = EditMode.ARC;
        }

        private void lineButton_CheckedChanged(object sender, EventArgs e)
        {
            editMode = EditMode.LINE;
        }

        private void selectButton_CheckedChanged(object sender, EventArgs e)
        {
            editMode = EditMode.SELECT;
        }

        private void extrudeButton_Click(object sender, EventArgs e)
        {
            // TODO begin extrude process
        }

        private void revolveButton_Click(object sender, EventArgs e)
        {

        }
    }
}