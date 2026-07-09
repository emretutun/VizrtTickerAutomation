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
            this.components = new System.ComponentModel.Container();
            this.btnHaberVer = new System.Windows.Forms.Button();
            this.pnl_TC_Connected = new System.Windows.Forms.Panel();
            this.tmr_connect = new System.Windows.Forms.Timer(this.components);
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
            // pnl_TC_Connected
            // 
            this.pnl_TC_Connected.BackColor = System.Drawing.Color.Red;
            this.pnl_TC_Connected.Location = new System.Drawing.Point(620, 12);
            this.pnl_TC_Connected.Name = "pnl_TC_Connected";
            this.pnl_TC_Connected.Size = new System.Drawing.Size(50, 51);
            this.pnl_TC_Connected.TabIndex = 1;
            // 
            // tmr_connect
            // 
            this.tmr_connect.Enabled = true;
            this.tmr_connect.Interval = 2000;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnl_TC_Connected);
            this.Controls.Add(this.btnHaberVer);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnHaberVer;
        private System.Windows.Forms.Panel pnl_TC_Connected;
        private System.Windows.Forms.Timer tmr_connect;
    }
}

