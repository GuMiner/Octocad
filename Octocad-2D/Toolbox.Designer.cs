namespace Octocad_2D
{
    partial class Toolbox
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
            this.actionsBox = new System.Windows.Forms.GroupBox();
            this.dimensionButton = new System.Windows.Forms.RadioButton();
            this.arcButton = new System.Windows.Forms.RadioButton();
            this.lineButton = new System.Windows.Forms.RadioButton();
            this.selectButton = new System.Windows.Forms.RadioButton();
            this.snapToGrid = new System.Windows.Forms.CheckBox();
            this.snapEndpoints = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.extrudeButton = new System.Windows.Forms.Button();
            this.revolveButton = new System.Windows.Forms.Button();
            this.intersectionRadio = new System.Windows.Forms.RadioButton();
            this.subtractRadio = new System.Windows.Forms.RadioButton();
            this.addRadio = new System.Windows.Forms.RadioButton();
            this.actionsBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // actionsBox
            // 
            this.actionsBox.Controls.Add(this.dimensionButton);
            this.actionsBox.Controls.Add(this.arcButton);
            this.actionsBox.Controls.Add(this.lineButton);
            this.actionsBox.Controls.Add(this.selectButton);
            this.actionsBox.Location = new System.Drawing.Point(13, 13);
            this.actionsBox.Name = "actionsBox";
            this.actionsBox.Size = new System.Drawing.Size(99, 110);
            this.actionsBox.TabIndex = 0;
            this.actionsBox.TabStop = false;
            this.actionsBox.Text = "Draw Actions";
            // 
            // dimensionButton
            // 
            this.dimensionButton.AutoSize = true;
            this.dimensionButton.Location = new System.Drawing.Point(3, 85);
            this.dimensionButton.Name = "dimensionButton";
            this.dimensionButton.Size = new System.Drawing.Size(74, 17);
            this.dimensionButton.TabIndex = 3;
            this.dimensionButton.TabStop = true;
            this.dimensionButton.Text = "Dimension";
            this.dimensionButton.UseVisualStyleBackColor = true;
            this.dimensionButton.CheckedChanged += new System.EventHandler(this.dimensionButton_CheckedChanged);
            // 
            // arcButton
            // 
            this.arcButton.AutoSize = true;
            this.arcButton.Location = new System.Drawing.Point(3, 62);
            this.arcButton.Name = "arcButton";
            this.arcButton.Size = new System.Drawing.Size(41, 17);
            this.arcButton.TabIndex = 2;
            this.arcButton.TabStop = true;
            this.arcButton.Text = "Arc";
            this.arcButton.UseVisualStyleBackColor = true;
            this.arcButton.CheckedChanged += new System.EventHandler(this.arcButton_CheckedChanged);
            // 
            // lineButton
            // 
            this.lineButton.AutoSize = true;
            this.lineButton.Location = new System.Drawing.Point(3, 39);
            this.lineButton.Name = "lineButton";
            this.lineButton.Size = new System.Drawing.Size(45, 17);
            this.lineButton.TabIndex = 1;
            this.lineButton.TabStop = true;
            this.lineButton.Text = "Line";
            this.lineButton.UseVisualStyleBackColor = true;
            this.lineButton.CheckedChanged += new System.EventHandler(this.lineButton_CheckedChanged);
            // 
            // selectButton
            // 
            this.selectButton.AutoSize = true;
            this.selectButton.Checked = true;
            this.selectButton.Location = new System.Drawing.Point(3, 16);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(55, 17);
            this.selectButton.TabIndex = 0;
            this.selectButton.TabStop = true;
            this.selectButton.Text = "Select";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.CheckedChanged += new System.EventHandler(this.selectButton_CheckedChanged);
            // 
            // snapToGrid
            // 
            this.snapToGrid.AutoSize = true;
            this.snapToGrid.Checked = true;
            this.snapToGrid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.snapToGrid.Location = new System.Drawing.Point(13, 129);
            this.snapToGrid.Name = "snapToGrid";
            this.snapToGrid.Size = new System.Drawing.Size(91, 17);
            this.snapToGrid.TabIndex = 1;
            this.snapToGrid.Text = "Snap to Grid?";
            this.snapToGrid.UseVisualStyleBackColor = true;
            this.snapToGrid.CheckedChanged += new System.EventHandler(this.snapToGrid_CheckedChanged);
            // 
            // snapEndpoints
            // 
            this.snapEndpoints.AutoSize = true;
            this.snapEndpoints.Checked = true;
            this.snapEndpoints.CheckState = System.Windows.Forms.CheckState.Checked;
            this.snapEndpoints.Location = new System.Drawing.Point(13, 153);
            this.snapEndpoints.Name = "snapEndpoints";
            this.snapEndpoints.Size = new System.Drawing.Size(107, 17);
            this.snapEndpoints.TabIndex = 2;
            this.snapEndpoints.Text = "Snap Endpoints?";
            this.snapEndpoints.UseVisualStyleBackColor = true;
            this.snapEndpoints.CheckedChanged += new System.EventHandler(this.snapEndpoints_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.intersectionRadio);
            this.groupBox1.Controls.Add(this.subtractRadio);
            this.groupBox1.Controls.Add(this.addRadio);
            this.groupBox1.Controls.Add(this.revolveButton);
            this.groupBox1.Controls.Add(this.extrudeButton);
            this.groupBox1.Location = new System.Drawing.Point(13, 177);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(99, 152);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "3D Create";
            // 
            // extrudeButton
            // 
            this.extrudeButton.Location = new System.Drawing.Point(3, 19);
            this.extrudeButton.Name = "extrudeButton";
            this.extrudeButton.Size = new System.Drawing.Size(74, 23);
            this.extrudeButton.TabIndex = 0;
            this.extrudeButton.Text = "Extrude";
            this.extrudeButton.UseVisualStyleBackColor = true;
            this.extrudeButton.Click += new System.EventHandler(this.extrudeButton_Click);
            // 
            // revolveButton
            // 
            this.revolveButton.Location = new System.Drawing.Point(3, 48);
            this.revolveButton.Name = "revolveButton";
            this.revolveButton.Size = new System.Drawing.Size(75, 23);
            this.revolveButton.TabIndex = 1;
            this.revolveButton.Text = "Revolve";
            this.revolveButton.UseVisualStyleBackColor = true;
            this.revolveButton.Click += new System.EventHandler(this.revolveButton_Click);
            // 
            // intersectionRadio
            // 
            this.intersectionRadio.AutoSize = true;
            this.intersectionRadio.Location = new System.Drawing.Point(6, 121);
            this.intersectionRadio.Name = "intersectionRadio";
            this.intersectionRadio.Size = new System.Drawing.Size(80, 17);
            this.intersectionRadio.TabIndex = 5;
            this.intersectionRadio.TabStop = true;
            this.intersectionRadio.Text = "Intersection";
            this.intersectionRadio.UseVisualStyleBackColor = true;
            // 
            // subtractRadio
            // 
            this.subtractRadio.AutoSize = true;
            this.subtractRadio.Location = new System.Drawing.Point(6, 98);
            this.subtractRadio.Name = "subtractRadio";
            this.subtractRadio.Size = new System.Drawing.Size(65, 17);
            this.subtractRadio.TabIndex = 4;
            this.subtractRadio.TabStop = true;
            this.subtractRadio.Text = "Subtract";
            this.subtractRadio.UseVisualStyleBackColor = true;
            // 
            // addRadio
            // 
            this.addRadio.AutoSize = true;
            this.addRadio.Checked = true;
            this.addRadio.Location = new System.Drawing.Point(6, 75);
            this.addRadio.Name = "addRadio";
            this.addRadio.Size = new System.Drawing.Size(44, 17);
            this.addRadio.TabIndex = 3;
            this.addRadio.TabStop = true;
            this.addRadio.Text = "Add";
            this.addRadio.UseVisualStyleBackColor = true;
            // 
            // Toolbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(124, 336);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.snapEndpoints);
            this.Controls.Add(this.snapToGrid);
            this.Controls.Add(this.actionsBox);
            this.Location = new System.Drawing.Point(710, 100);
            this.Name = "Toolbox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "2D Toolbox";
            this.actionsBox.ResumeLayout(false);
            this.actionsBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox actionsBox;
        private System.Windows.Forms.RadioButton selectButton;
        private System.Windows.Forms.RadioButton dimensionButton;
        private System.Windows.Forms.RadioButton arcButton;
        private System.Windows.Forms.RadioButton lineButton;
        private System.Windows.Forms.CheckBox snapToGrid;
        private System.Windows.Forms.CheckBox snapEndpoints;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button extrudeButton;
        private System.Windows.Forms.RadioButton intersectionRadio;
        private System.Windows.Forms.RadioButton subtractRadio;
        private System.Windows.Forms.RadioButton addRadio;
        private System.Windows.Forms.Button revolveButton;
    }
}