using System;
using System.Windows.Forms;
using Common;

namespace TickerUI
{
    public partial class Form1 : Form
    {
        private CSession _serviceConnection;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Servisimizin çalıştığı adrese (localhost) bağlanıyoruz
            _serviceConnection = new CSession("127.0.0.1", 6405);
            _serviceConnection.OnConnect += client =>
                this.Invoke((MethodInvoker)delegate { Console.WriteLine("Servise Bağlandı!"); });

            _serviceConnection.Connect();
        }

        private void btnHaberVer_Click(object sender, EventArgs e)
        {
            // Servise sadece komut metnini gönderiyoruz
            if (_serviceConnection != null)
            {
                _serviceConnection.SendText("Haber Ticker Ver");
            }
        }
    }
}