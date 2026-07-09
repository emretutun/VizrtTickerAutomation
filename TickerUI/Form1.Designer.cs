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
            this.label1 = new System.Windows.Forms.Label();
            this.tbxSonDakika = new System.Windows.Forms.TextBox();
            this.btnSonDakika = new System.Windows.Forms.Button();
            this.btnTekrar = new System.Windows.Forms.Button();
            this.btnCanli = new System.Windows.Forms.Button();
            this.btnLogoVer = new System.Windows.Forms.Button();
            this.btnReklamLogoVer = new System.Windows.Forms.Button();
            this.btnLogoAl = new System.Windows.Forms.Button();
            this.tbxSondkBant = new System.Windows.Forms.TextBox();
            this.btnSondkBant = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnHaberVer
            // 
            this.btnHaberVer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHaberVer.Location = new System.Drawing.Point(16, 136);
            this.btnHaberVer.Name = "btnHaberVer";
            this.btnHaberVer.Size = new System.Drawing.Size(158, 59);
            this.btnHaberVer.TabIndex = 0;
            this.btnHaberVer.Text = "Ticker Ver";
            this.btnHaberVer.UseVisualStyleBackColor = true;
            this.btnHaberVer.Click += new System.EventHandler(this.btnHaberVer_Click);
            // 
            // pnl_TC_Connected
            // 
            this.pnl_TC_Connected.BackColor = System.Drawing.Color.Red;
            this.pnl_TC_Connected.Location = new System.Drawing.Point(225, 23);
            this.pnl_TC_Connected.Name = "pnl_TC_Connected";
            this.pnl_TC_Connected.Size = new System.Drawing.Size(16, 16);
            this.pnl_TC_Connected.TabIndex = 1;
            // 
            // tmr_connect
            // 
            this.tmr_connect.Enabled = true;
            this.tmr_connect.Interval = 2000;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Servis Bağlantı Durumu :";
            // 
            // tbxSonDakika
            // 
            this.tbxSonDakika.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxSonDakika.Location = new System.Drawing.Point(485, 176);
            this.tbxSonDakika.Multiline = true;
            this.tbxSonDakika.Name = "tbxSonDakika";
            this.tbxSonDakika.Size = new System.Drawing.Size(313, 29);
            this.tbxSonDakika.TabIndex = 3;
            // 
            // btnSonDakika
            // 
            this.btnSonDakika.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSonDakika.Location = new System.Drawing.Point(553, 211);
            this.btnSonDakika.Name = "btnSonDakika";
            this.btnSonDakika.Size = new System.Drawing.Size(157, 59);
            this.btnSonDakika.TabIndex = 4;
            this.btnSonDakika.Text = "Son Dakika KJ Ver";
            this.btnSonDakika.UseVisualStyleBackColor = true;
            this.btnSonDakika.Click += new System.EventHandler(this.btnSonDakika_Click);
            // 
            // btnTekrar
            // 
            this.btnTekrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTekrar.Location = new System.Drawing.Point(164, 225);
            this.btnTekrar.Name = "btnTekrar";
            this.btnTekrar.Size = new System.Drawing.Size(119, 45);
            this.btnTekrar.TabIndex = 6;
            this.btnTekrar.Text = "Tekrar Ver";
            this.btnTekrar.UseVisualStyleBackColor = true;
            this.btnTekrar.Click += new System.EventHandler(this.btnTekrar_Click);
            // 
            // btnCanli
            // 
            this.btnCanli.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCanli.Location = new System.Drawing.Point(37, 225);
            this.btnCanli.Name = "btnCanli";
            this.btnCanli.Size = new System.Drawing.Size(121, 45);
            this.btnCanli.TabIndex = 7;
            this.btnCanli.Text = "Canlı Ver";
            this.btnCanli.UseVisualStyleBackColor = true;
            this.btnCanli.Click += new System.EventHandler(this.btnCanli_Click);
            // 
            // btnLogoVer
            // 
            this.btnLogoVer.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogoVer.Location = new System.Drawing.Point(575, 23);
            this.btnLogoVer.Name = "btnLogoVer";
            this.btnLogoVer.Size = new System.Drawing.Size(75, 63);
            this.btnLogoVer.TabIndex = 8;
            this.btnLogoVer.Text = "LOGO VER";
            this.btnLogoVer.UseVisualStyleBackColor = true;
            this.btnLogoVer.Click += new System.EventHandler(this.btnLogoVer_Click);
            // 
            // btnReklamLogoVer
            // 
            this.btnReklamLogoVer.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReklamLogoVer.Location = new System.Drawing.Point(656, 23);
            this.btnReklamLogoVer.Name = "btnReklamLogoVer";
            this.btnReklamLogoVer.Size = new System.Drawing.Size(75, 63);
            this.btnReklamLogoVer.TabIndex = 9;
            this.btnReklamLogoVer.Text = "Reklam Logo Ver";
            this.btnReklamLogoVer.UseVisualStyleBackColor = true;
            this.btnReklamLogoVer.Click += new System.EventHandler(this.btnReklamLogoVer_Click);
            // 
            // btnLogoAl
            // 
            this.btnLogoAl.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogoAl.Location = new System.Drawing.Point(610, 92);
            this.btnLogoAl.Name = "btnLogoAl";
            this.btnLogoAl.Size = new System.Drawing.Size(75, 28);
            this.btnLogoAl.TabIndex = 10;
            this.btnLogoAl.Text = "AL";
            this.btnLogoAl.UseVisualStyleBackColor = true;
            this.btnLogoAl.Click += new System.EventHandler(this.btnLogoAl_Click);
            // 
            // tbxSondkBant
            // 
            this.tbxSondkBant.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxSondkBant.Location = new System.Drawing.Point(37, 334);
            this.tbxSondkBant.Multiline = true;
            this.tbxSondkBant.Name = "tbxSondkBant";
            this.tbxSondkBant.Size = new System.Drawing.Size(719, 29);
            this.tbxSondkBant.TabIndex = 11;
            // 
            // btnSondkBant
            // 
            this.btnSondkBant.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSondkBant.Location = new System.Drawing.Point(323, 369);
            this.btnSondkBant.Name = "btnSondkBant";
            this.btnSondkBant.Size = new System.Drawing.Size(154, 46);
            this.btnSondkBant.TabIndex = 12;
            this.btnSondkBant.Text = "SonDk Ticker Ver";
            this.btnSondkBant.UseVisualStyleBackColor = true;
            this.btnSondkBant.Click += new System.EventHandler(this.btnSondkBant_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnSondkBant);
            this.Controls.Add(this.tbxSondkBant);
            this.Controls.Add(this.btnLogoAl);
            this.Controls.Add(this.btnReklamLogoVer);
            this.Controls.Add(this.btnLogoVer);
            this.Controls.Add(this.btnCanli);
            this.Controls.Add(this.btnTekrar);
            this.Controls.Add(this.btnSonDakika);
            this.Controls.Add(this.tbxSonDakika);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnl_TC_Connected);
            this.Controls.Add(this.btnHaberVer);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnHaberVer;
        private System.Windows.Forms.Panel pnl_TC_Connected;
        private System.Windows.Forms.Timer tmr_connect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxSonDakika;
        private System.Windows.Forms.Button btnSonDakika;
        private System.Windows.Forms.Button btnTekrar;
        private System.Windows.Forms.Button btnCanli;
        private System.Windows.Forms.Button btnLogoVer;
        private System.Windows.Forms.Button btnReklamLogoVer;
        private System.Windows.Forms.Button btnLogoAl;
        private System.Windows.Forms.TextBox tbxSondkBant;
        private System.Windows.Forms.Button btnSondkBant;
    }
}

