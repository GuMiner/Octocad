using System;
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
    partial class Preferences : Form
    {
        public static double length, resolution;
        public static String units;

        private ProcessLink cppLink;

        public Preferences(ProcessLink processLink)
        {
            InitializeComponent();
            resetButton_Click(null, null);

            cppLink = processLink;
        }

        public static double GetErrorResolution()
        {
            return resolution / 10; // In board coordinates
        }

        public static double GetScreenErrorResolution()
        {
            return 10; // In pixels
        }

        private void RefreshUI()
        {
            lengthBox.Text = String.Format("{0}", length);
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
            unitBox4.Text = unitBox1.Text;
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            this.Hide();

            // TODO pass in units separately
            cppLink.WriteToOctocadCpp(MessageHandler.TranslateMessage(MessageHandler.MessageType.PREFERENCES_UPDATE, length, resolution));
        }

        /// <summary>
        /// Resets the provided parameters to their defaults.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resetButton_Click(object sender, EventArgs e)
        {
            length = 120.0;
            resolution = 0.1;
            units = "mm";

            RefreshUI();
        }
    }
}
