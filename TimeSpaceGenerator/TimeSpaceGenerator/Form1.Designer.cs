namespace TimeSpaceGenerator
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.XmlOutputBox = new System.Windows.Forms.TextBox();
            this.PacketTextBox = new System.Windows.Forms.TextBox();
            this.GenerateXmlButton = new System.Windows.Forms.Button();
            this.PacketsLabel = new System.Windows.Forms.Label();
            this.XmlOutput = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // XmlOutputBox
            // 
            this.XmlOutputBox.Location = new System.Drawing.Point(428, 112);
            this.XmlOutputBox.Multiline = true;
            XmlOutputBox.ReadOnly = true;
            this.XmlOutputBox.Name = "XmlOutputBox";
            this.XmlOutputBox.Size = new System.Drawing.Size(572, 568);
            this.XmlOutputBox.TabIndex = 0;
            // 
            // PacketTextBox
            // 
            this.PacketTextBox.Location = new System.Drawing.Point(12, 112);
            this.PacketTextBox.Multiline = true;
            this.PacketTextBox.Name = "PacketTextBox";
            this.PacketTextBox.Size = new System.Drawing.Size(396, 196);
            this.PacketTextBox.TabIndex = 1;
            // 
            // GenerateXmlButton
            // 
            this.GenerateXmlButton.Location = new System.Drawing.Point(16, 325);
            this.GenerateXmlButton.Name = "GenerateXmlButton";
            this.GenerateXmlButton.Size = new System.Drawing.Size(155, 59);
            this.GenerateXmlButton.TabIndex = 2;
            this.GenerateXmlButton.Text = "Generate";
            this.GenerateXmlButton.UseVisualStyleBackColor = true;
            this.GenerateXmlButton.Click += new System.EventHandler(this.GenerateXmlButton_Click);
            // 
            // PacketsLabel
            // 
            this.PacketsLabel.AutoSize = true;
            this.PacketsLabel.Location = new System.Drawing.Point(9, 87);
            this.PacketsLabel.Name = "PacketsLabel";
            this.PacketsLabel.Size = new System.Drawing.Size(46, 13);
            this.PacketsLabel.TabIndex = 3;
            this.PacketsLabel.Text = "Packets";
            // 
            // XmlOutput
            // 
            this.XmlOutput.AutoSize = true;
            this.XmlOutput.Location = new System.Drawing.Point(425, 87);
            this.XmlOutput.Name = "XmlOutput";
            this.XmlOutput.Size = new System.Drawing.Size(56, 13);
            this.XmlOutput.TabIndex = 4;
            this.XmlOutput.Text = "XmlOutput";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 692);
            this.Controls.Add(this.XmlOutput);
            this.Controls.Add(this.PacketsLabel);
            this.Controls.Add(this.GenerateXmlButton);
            this.Controls.Add(this.PacketTextBox);
            this.Controls.Add(this.XmlOutputBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "TimeSpace Generator by Elendan";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox XmlOutputBox;
        private System.Windows.Forms.TextBox PacketTextBox;
        private System.Windows.Forms.Button GenerateXmlButton;
        private System.Windows.Forms.Label PacketsLabel;
        private System.Windows.Forms.Label XmlOutput;
    }
}

