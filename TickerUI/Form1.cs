using System;
using System.Drawing;
using System.Windows.Forms;
using Common;

namespace TickerUI
{
    public partial class Form1 : Form
    {
        private CSession _serviceConnection;
        private bool _connectLock = false;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;

            // Timer eventini bağlıyoruz
            tmr_connect.Tick += tmr_connect_Tick;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Bağlantı nesnemizi oluşturuyoruz ama hemen bağlanmıyoruz, Timer halledecek
            _serviceConnection = new CSession("127.0.0.1", 6405);

            // Bağlantı başarılı olduğunda (Yeşil yanacak)
            _serviceConnection.OnConnect += client =>
                this.Invoke((MethodInvoker)delegate {
                    pnl_TC_Connected.BackColor = Color.LimeGreen;
                    Console.WriteLine("Servise Bağlandı!");
                });

            // Bağlantı koptuğunda (Kırmızı yanacak)
            _serviceConnection.OnDisconnect += client =>
                this.Invoke((MethodInvoker)delegate {
                    pnl_TC_Connected.BackColor = Color.Firebrick;
                    Console.WriteLine("Servis Bağlantısı Koptu!");
                });
        }

        private void tmr_connect_Tick(object sender, EventArgs e)
        {
            // Aynı anda birden fazla bağlanma isteği atılmasını engellemek için kilit
            if (_connectLock) return;

            _connectLock = true;
            try
            {
                // Eğer bağlantı koptuysa veya henüz kurulamadıysa bağlanmayı dene
                if (_serviceConnection != null && pnl_TC_Connected.BackColor != Color.LimeGreen)
                {
                    _serviceConnection.Connect(6405);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bağlantı denemesi başarısız: " + ex.Message);
            }
            finally
            {
                _connectLock = false;
            }
        }

        private void btnHaberVer_Click(object sender, EventArgs e)
        {
            if (pnl_TC_Connected.BackColor == Color.LimeGreen)
            {
                _serviceConnection.SendText("Haber Ticker Ver");
            }
            else
            {
                MessageBox.Show("Servis ile bağlantı yok! Lütfen servisin çalıştığından emin olun.", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}