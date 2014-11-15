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
    public partial class Preferences : Form
    {
        public static double length, width, height, resolution;
        public static String units;

        public Preferences()
        {
            InitializeComponent();
            resetButton_Click(null, null);
        }

        private void RefreshUI()
        {
            lengthBox.Text = String.Format("{0}", length);
            widthBox.Text = String.Format("{0}", width);
            heightBox.Text = String.Format("{0}", height);
            resolutionBox.Text = String.Format("{0}", resolution);
            for (int i = 0; i < unitComboBox.Items.Count; i++)
            {
                if (units.Equals(unitComboBox.Items[0].ToString()))
                {
                    unitComboBox.SelectedIndex = i;
                    break;
                }
            }
        }

        /// <summary>
        /// Updates the displayed units based upon the selected unit provided
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void unitComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            unitBox1.Text = unitComboBox.SelectedItem.ToString();
            unitBox2.Text = unitBox1.Text;
            unitBox3.Text = unitBox1.Text;
            unitBox4.Text = unitBox1.Text;
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// Resets the provided parameters to their defaults.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resetButton_Click(object sender, EventArgs e)
        {
            length = 120.0;
            width = 100.0;
            height = 80.0;
            resolution = 0.1;
            units = "mm";

            RefreshUI();
        }
    }
}