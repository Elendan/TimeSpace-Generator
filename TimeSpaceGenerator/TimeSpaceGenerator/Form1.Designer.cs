using System.Drawing;
using System.Windows.Forms;

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
            this.ErrorTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.RbrPacketTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ExitButton = new System.Windows.Forms.Button();
            this.XmlFileNameTextBox = new System.Windows.Forms.TextBox();
            this.GenerateXmlFileButton = new System.Windows.Forms.Button();
            this.LabelTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // XmlOutputBox
            // 
            this.XmlOutputBox.Location = new System.Drawing.Point(428, 25);
            this.XmlOutputBox.Multiline = true;
            this.XmlOutputBox.Name = "XmlOutputBox";
            this.XmlOutputBox.ReadOnly = true;
            this.XmlOutputBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.XmlOutputBox.Size = new System.Drawing.Size(572, 568);
            this.XmlOutputBox.TabIndex = 0;
            // 
            // PacketTextBox
            // 
            this.PacketTextBox.Location = new System.Drawing.Point(12, 191);
            this.PacketTextBox.Multiline = true;
            this.PacketTextBox.Name = "PacketTextBox";
            this.PacketTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.PacketTextBox.Size = new System.Drawing.Size(396, 196);
            this.PacketTextBox.TabIndex = 1;
            // 
            // GenerateXmlButton
            // 
            this.GenerateXmlButton.Location = new System.Drawing.Point(12, 393);
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
            this.PacketsLabel.Location = new System.Drawing.Point(9, 175);
            this.PacketsLabel.Name = "PacketsLabel";
            this.PacketsLabel.Size = new System.Drawing.Size(46, 13);
            this.PacketsLabel.TabIndex = 3;
            this.PacketsLabel.Text = "Packets";
            this.PacketsLabel.Click += new System.EventHandler(this.PacketsLabel_Click);
            // 
            // XmlOutput
            // 
            this.XmlOutput.AutoSize = true;
            this.XmlOutput.Location = new System.Drawing.Point(425, 9);
            this.XmlOutput.Name = "XmlOutput";
            this.XmlOutput.Size = new System.Drawing.Size(56, 13);
            this.XmlOutput.TabIndex = 4;
            this.XmlOutput.Text = "XmlOutput";
            // 
            // ErrorTextBox
            // 
            this.ErrorTextBox.Location = new System.Drawing.Point(12, 471);
            this.ErrorTextBox.Multiline = true;
            this.ErrorTextBox.Name = "ErrorTextBox";
            this.ErrorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ErrorTextBox.Size = new System.Drawing.Size(392, 209);
            this.ErrorTextBox.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 455);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Error Log";
            // 
            // RbrPacketTextBox
            // 
            this.RbrPacketTextBox.Location = new System.Drawing.Point(16, 25);
            this.RbrPacketTextBox.Name = "RbrPacketTextBox";
            this.RbrPacketTextBox.Size = new System.Drawing.Size(395, 20);
            this.RbrPacketTextBox.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "rbr packet";
            // 
            // ExitButton
            // 
            this.ExitButton.AutoSize = true;
            this.ExitButton.BackColor = System.Drawing.Color.White;
            this.ExitButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ExitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ExitButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ExitButton.Location = new System.Drawing.Point(959, 12);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(20, 20);
            this.ExitButton.TabIndex = 11;
            this.ExitButton.Text = "X";
            this.ExitButton.UseVisualStyleBackColor = false;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // XmlFileNameTextBox
            // 
            this.XmlFileNameTextBox.Location = new System.Drawing.Point(788, 653);
            this.XmlFileNameTextBox.Name = "XmlFileNameTextBox";
            this.XmlFileNameTextBox.Size = new System.Drawing.Size(211, 20);
            this.XmlFileNameTextBox.TabIndex = 9;
            // 
            // GenerateXmlFileButton
            // 
            this.GenerateXmlFileButton.Location = new System.Drawing.Point(680, 653);
            this.GenerateXmlFileButton.Name = "GenerateXmlFileButton";
            this.GenerateXmlFileButton.Size = new System.Drawing.Size(98, 19);
            this.GenerateXmlFileButton.TabIndex = 10;
            this.GenerateXmlFileButton.Text = "Generate File";
            this.GenerateXmlFileButton.UseVisualStyleBackColor = true;
            this.GenerateXmlFileButton.Click += new System.EventHandler(this.GenerateXmlFileButton_Click);
            // 
            // LabelTextBox
            // 
            this.LabelTextBox.Location = new System.Drawing.Point(13, 98);
            this.LabelTextBox.Name = "LabelTextBox";
            this.LabelTextBox.Size = new System.Drawing.Size(395, 20);
            this.LabelTextBox.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Description";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 692);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LabelTextBox);
            this.Controls.Add(this.GenerateXmlFileButton);
            this.Controls.Add(this.XmlFileNameTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.RbrPacketTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ErrorTextBox);
            this.Controls.Add(this.XmlOutput);
            this.Controls.Add(this.PacketsLabel);
            this.Controls.Add(this.GenerateXmlButton);
            this.Controls.Add(this.PacketTextBox);
            this.Controls.Add(this.XmlOutputBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "TimeSpace Generator by Elendan";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox XmlOutputBox;
        private System.Windows.Forms.TextBox PacketTextBox;
        private System.Windows.Forms.Button GenerateXmlButton;
        private System.Windows.Forms.Label PacketsLabel;
        private System.Windows.Forms.Label XmlOutput;
        private System.Windows.Forms.TextBox ErrorTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox RbrPacketTextBox;
        private System.Windows.Forms.Label label2;
        private Button ExitButton;
        private TextBox XmlFileNameTextBox;
        private Button GenerateXmlFileButton;
        private TextBox LabelTextBox;
        private Label label3;
    }
}

