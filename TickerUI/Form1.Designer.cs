namespace TickerUI
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
            this.btnHaberVer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnHaberVer
            // 
            this.btnHaberVer.Location = new System.Drawing.Point(276, 12);
            this.btnHaberVer.Name = "btnHaberVer";
            this.btnHaberVer.Size = new System.Drawing.Size(95, 36);
            this.btnHaberVer.TabIndex = 0;
            this.btnHaberVer.Text = "Haber Ver";
            this.btnHaberVer.UseVisualStyleBackColor = true;
            this.btnHaberVer.Click += new System.EventHandler(this.btnHaberVer_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnHaberVer);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnHaberVer;
    }
}

