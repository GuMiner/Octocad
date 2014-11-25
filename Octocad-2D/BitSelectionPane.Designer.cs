namespace Octocad_2D
{
    partial class BitSelectionPane
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.bitmapPanel = new System.Windows.Forms.Panel();
            this.resetButton = new System.Windows.Forms.Button();
            this.isMirrored = new System.Windows.Forms.CheckBox();
            this.distanceBox = new System.Windows.Forms.TextBox();
            this.Distance = new System.Windows.Forms.Label();
            this.unitLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(501, 441);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(420, 441);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // bitmapPanel
            // 
            this.bitmapPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bitmapPanel.Location = new System.Drawing.Point(12, 12);
            this.bitmapPanel.Name = "bitmapPanel";
            this.bitmapPanel.Size = new System.Drawing.Size(564, 423);
            this.bitmapPanel.TabIndex = 2;
            this.bitmapPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.bitmapPanel_Paint);
            this.bitmapPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.bitmapPanel_MouseClick);
            // 
            // resetButton
            // 
            this.resetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.resetButton.Location = new System.Drawing.Point(12, 441);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(103, 23);
            this.resetButton.TabIndex = 3;
            this.resetButton.Text = "Reset Selection";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // isMirrored
            // 
            this.isMirrored.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.isMirrored.AutoSize = true;
            this.isMirrored.Location = new System.Drawing.Point(344, 445);
            this.isMirrored.Name = "isMirrored";
            this.isMirrored.Size = new System.Drawing.Size(70, 17);
            this.isMirrored.TabIndex = 4;
            this.isMirrored.Text = "Mirrored?";
            this.isMirrored.UseVisualStyleBackColor = true;
            // 
            // distanceBox
            // 
            this.distanceBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.distanceBox.Location = new System.Drawing.Point(202, 443);
            this.distanceBox.Name = "distanceBox";
            this.distanceBox.Size = new System.Drawing.Size(100, 20);
            this.distanceBox.TabIndex = 5;
            this.distanceBox.Text = "1.0";
            this.distanceBox.TextChanged += new System.EventHandler(this.distanceBox_TextChanged);
            // 
            // Distance
            // 
            this.Distance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Distance.AutoSize = true;
            this.Distance.Location = new System.Drawing.Point(144, 446);
            this.Distance.Name = "Distance";
            this.Distance.Size = new System.Drawing.Size(52, 13);
            this.Distance.TabIndex = 6;
            this.Distance.Text = "Distance:";
            // 
            // unitLabel
            // 
            this.unitLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.unitLabel.AutoSize = true;
            this.unitLabel.Location = new System.Drawing.Point(308, 446);
            this.unitLabel.Name = "unitLabel";
            this.unitLabel.Size = new System.Drawing.Size(27, 13);
            this.unitLabel.TabIndex = 7;
            this.unitLabel.Text = "amft";
            // 
            // BitSelectionPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 476);
            this.ControlBox = false;
            this.Controls.Add(this.unitLabel);
            this.Controls.Add(this.Distance);
            this.Controls.Add(this.distanceBox);
            this.Controls.Add(this.isMirrored);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.bitmapPanel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.DoubleBuffered = true;
            this.Name = "BitSelectionPane";
            this.Text = "BitSelectionPane";
            this.Resize += new System.EventHandler(this.BitSelectionPane_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Panel bitmapPanel;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.CheckBox isMirrored;
        private System.Windows.Forms.TextBox distanceBox;
        private System.Windows.Forms.Label Distance;
        private System.Windows.Forms.Label unitLabel;
    }
}