namespace Octocad_2D
{
    partial class Preferences
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
            this.resetButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lengthBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.widthBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.heightBox = new System.Windows.Forms.TextBox();
            this.unitBox1 = new System.Windows.Forms.Label();
            this.unitBox2 = new System.Windows.Forms.Label();
            this.unitBox3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.unitComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.resolutionBox = new System.Windows.Forms.TextBox();
            this.unitBox4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(12, 141);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(106, 23);
            this.resetButton.TabIndex = 0;
            this.resetButton.Text = "Reset to Defaults";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Location = new System.Drawing.Point(251, 141);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(75, 23);
            this.acceptButton.TabIndex = 1;
            this.acceptButton.Text = "Accept";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.unitBox4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.resolutionBox);
            this.groupBox1.Controls.Add(this.unitComboBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.unitBox3);
            this.groupBox1.Controls.Add(this.unitBox2);
            this.groupBox1.Controls.Add(this.unitBox1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.heightBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.widthBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lengthBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(314, 122);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Work Area Settings";
            // 
            // lengthBox
            // 
            this.lengthBox.Location = new System.Drawing.Point(71, 19);
            this.lengthBox.Name = "lengthBox";
            this.lengthBox.Size = new System.Drawing.Size(100, 20);
            this.lengthBox.TabIndex = 0;
            this.lengthBox.Text = "120.0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Length (X):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Width (Y):";
            // 
            // widthBox
            // 
            this.widthBox.Location = new System.Drawing.Point(71, 45);
            this.widthBox.Name = "widthBox";
            this.widthBox.Size = new System.Drawing.Size(100, 20);
            this.widthBox.TabIndex = 2;
            this.widthBox.Text = "100.0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Height (Z):";
            // 
            // heightBox
            // 
            this.heightBox.Location = new System.Drawing.Point(71, 71);
            this.heightBox.Name = "heightBox";
            this.heightBox.Size = new System.Drawing.Size(100, 20);
            this.heightBox.TabIndex = 4;
            this.heightBox.Text = "80.0";
            // 
            // unitBox1
            // 
            this.unitBox1.AutoSize = true;
            this.unitBox1.Location = new System.Drawing.Point(177, 22);
            this.unitBox1.Name = "unitBox1";
            this.unitBox1.Size = new System.Drawing.Size(23, 13);
            this.unitBox1.TabIndex = 6;
            this.unitBox1.Text = "mm";
            // 
            // unitBox2
            // 
            this.unitBox2.AutoSize = true;
            this.unitBox2.Location = new System.Drawing.Point(177, 48);
            this.unitBox2.Name = "unitBox2";
            this.unitBox2.Size = new System.Drawing.Size(23, 13);
            this.unitBox2.TabIndex = 7;
            this.unitBox2.Text = "mm";
            // 
            // unitBox3
            // 
            this.unitBox3.AutoSize = true;
            this.unitBox3.Location = new System.Drawing.Point(177, 74);
            this.unitBox3.Name = "unitBox3";
            this.unitBox3.Size = new System.Drawing.Size(23, 13);
            this.unitBox3.TabIndex = 8;
            this.unitBox3.Text = "mm";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(222, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Units:";
            // 
            // unitComboBox
            // 
            this.unitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.unitComboBox.FormattingEnabled = true;
            this.unitComboBox.Items.AddRange(new object[] {
            "mm",
            "cm",
            "m",
            "in",
            "ft"});
            this.unitComboBox.Location = new System.Drawing.Point(256, 16);
            this.unitComboBox.Name = "unitComboBox";
            this.unitComboBox.Size = new System.Drawing.Size(52, 21);
            this.unitComboBox.TabIndex = 10;
            this.unitComboBox.SelectedIndexChanged += new System.EventHandler(this.unitComboBox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Resolution";
            // 
            // resolutionBox
            // 
            this.resolutionBox.Location = new System.Drawing.Point(71, 94);
            this.resolutionBox.Name = "resolutionBox";
            this.resolutionBox.Size = new System.Drawing.Size(100, 20);
            this.resolutionBox.TabIndex = 11;
            this.resolutionBox.Text = "0.1";
            // 
            // unitBox4
            // 
            this.unitBox4.AutoSize = true;
            this.unitBox4.Location = new System.Drawing.Point(177, 97);
            this.unitBox4.Name = "unitBox4";
            this.unitBox4.Size = new System.Drawing.Size(23, 13);
            this.unitBox4.TabIndex = 13;
            this.unitBox4.Text = "mm";
            // 
            // Preferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 175);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.resetButton);
            this.Name = "Preferences";
            this.Text = "Preferences";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox unitComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label unitBox3;
        private System.Windows.Forms.Label unitBox2;
        private System.Windows.Forms.Label unitBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox heightBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox widthBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox lengthBox;
        private System.Windows.Forms.Label unitBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox resolutionBox;
    }
}