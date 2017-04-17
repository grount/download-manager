using System;

namespace download_manager
{
    partial class OptionsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.maxThreadCountTrackBar = new System.Windows.Forms.TrackBar();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.threadCountTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.maxThreadCountTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Max simultaneous downloads:";
            // 
            // maxThreadCountTrackBar
            // 
            this.maxThreadCountTrackBar.Location = new System.Drawing.Point(166, 12);
            this.maxThreadCountTrackBar.Maximum = 4;
            this.maxThreadCountTrackBar.Minimum = 1;
            this.maxThreadCountTrackBar.Name = "maxThreadCountTrackBar";
            this.maxThreadCountTrackBar.Size = new System.Drawing.Size(104, 45);
            this.maxThreadCountTrackBar.TabIndex = 1;
            this.maxThreadCountTrackBar.Value = 1;
            this.maxThreadCountTrackBar.ValueChanged += new System.EventHandler(this.maxThreadCount_ValueChanged);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(70, 191);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(151, 191);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // threadCountTextBox
            // 
            this.threadCountTextBox.BackColor = System.Drawing.SystemColors.HighlightText;
            this.threadCountTextBox.Location = new System.Drawing.Point(277, 13);
            this.threadCountTextBox.Name = "threadCountTextBox";
            this.threadCountTextBox.ReadOnly = true;
            this.threadCountTextBox.Size = new System.Drawing.Size(22, 20);
            this.threadCountTextBox.TabIndex = 4;
            this.threadCountTextBox.Text = "1";
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 226);
            this.Controls.Add(this.threadCountTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.maxThreadCountTrackBar);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            ((System.ComponentModel.ISupportInitialize)(this.maxThreadCountTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar maxThreadCountTrackBar;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox threadCountTextBox;
    }
}