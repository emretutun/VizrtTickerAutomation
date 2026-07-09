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
        private bool _isTickerOn = false; //Ticker Yayında olup olmadığını kontrol etmek için
        private bool _isSonDakikaOn = false;
        private bool _isCanliOn = false;
        private bool _isTekrarOn = false;
        private bool _isSondkBantOn = false;

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
                if (!_isTickerOn)
                {
                    // Ticker yayında DEĞİL, yayına VER!
                    _serviceConnection.SendText("Haber Ticker Ver");
                    _isTickerOn = true;
                    btnHaberVer.Text = "Haber Ticker Al";
                    btnHaberVer.BackColor = Color.Firebrick;
                }
                else
                {
                    // Ticker YAYINDA, yayından AL!
                    _serviceConnection.SendText("Haber Ticker Al");
                    _isTickerOn = false;
                    btnHaberVer.Text = "Haber Ticker Ver";
                    btnHaberVer.BackColor = Color.Green;
                }
            }
            else
            {
                MessageBox.Show("Servis bağlantısı yok!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSonDakika_Click(object sender, EventArgs e)
        {
            if (pnl_TC_Connected.BackColor != Color.LimeGreen) return;

            if (!_isSonDakikaOn)
            {
                if (string.IsNullOrWhiteSpace(tbxSonDakika.Text))
                {
                    MessageBox.Show("Lütfen Son Dakika metni giriniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Servise "SET:" önekiyle dinamik metin gönderiyoruz
                _serviceConnection.SendText($"SET:SONDK KJ:{tbxSonDakika.Text.Trim()}");
                _isSonDakikaOn = true;
                btnSonDakika.Text = "Son Dakika Al";
                btnSonDakika.BackColor = Color.Firebrick;
            }
            else
            {
                _serviceConnection.SendText("SONDK KJ AL");
                _isSonDakikaOn = false;
                btnSonDakika.Text = "Son Dakika Ver";
                btnSonDakika.BackColor = Color.Green;
            }
        }

        private void btnCanli_Click(object sender, EventArgs e)
        {
            if (pnl_TC_Connected.BackColor != Color.LimeGreen) return;

            if (!_isCanliOn)
            {
                _serviceConnection.SendText("CANLI VER");
                _isCanliOn = true;
                btnCanli.Text = "Canlı Al";
                btnCanli.BackColor = Color.Firebrick;

                // Eğer Tekrar yayındaysa onu kapat
                if (_isTekrarOn) { _isTekrarOn = false; btnTekrar.Text = "Tekrar Ver"; btnTekrar.BackColor = SystemColors.Control; }
            }
            else
            {
                _serviceConnection.SendText("CANLI AL");
                _isCanliOn = false;
                btnCanli.Text = "Canlı Ver";
                btnCanli.BackColor = SystemColors.Control;
            }
        }

        private void btnTekrar_Click(object sender, EventArgs e)
        {
            if (pnl_TC_Connected.BackColor != Color.LimeGreen) return;

            if (!_isTekrarOn)
            {
                _serviceConnection.SendText("TEKRAR VER");
                _isTekrarOn = true;
                btnTekrar.Text = "Tekrar Al";
                btnTekrar.BackColor = Color.Firebrick;

                // Eğer Canlı yayındaysa onu kapat
                if (_isCanliOn) { _isCanliOn = false; btnCanli.Text = "Canlı Ver"; btnCanli.BackColor = SystemColors.Control; }
            }
            else
            {
                // Tekrar da aslında aynı grafiği (Canlı grafiğini) kapattığı için aynı komutu kullanabiliriz
                _serviceConnection.SendText("CANLI AL");
                _isTekrarOn = false;
                btnTekrar.Text = "Tekrar Ver";
                btnTekrar.BackColor = SystemColors.Control;
            }
        }

        private void btnLogoVer_Click(object sender, EventArgs e)
        {
            if (pnl_TC_Connected.BackColor != Color.LimeGreen) return;

            _serviceConnection.SendText("LOGO VER");

            // Renkleri ayarla: Logo kırmızı (yayında), Reklam normal
            btnLogoVer.BackColor = Color.Firebrick;
            btnReklamLogoVer.BackColor = SystemColors.Control;
        }

        private void btnReklamLogoVer_Click(object sender, EventArgs e)
        {
            if (pnl_TC_Connected.BackColor != Color.LimeGreen) return;

            _serviceConnection.SendText("REKLAM LOGO VER");

            // Renkleri ayarla: Reklam kırmızı (yayında), Logo normal
            btnReklamLogoVer.BackColor = Color.Firebrick;
            btnLogoVer.BackColor = SystemColors.Control;
        }

        private void btnLogoAl_Click(object sender, EventArgs e)
        {
            if (pnl_TC_Connected.BackColor != Color.LimeGreen) return;

            _serviceConnection.SendText("LOGO AL");

            // İkisi de yayından çıktığı için buton renklerini sıfırla
            btnLogoVer.BackColor = SystemColors.Control;
            btnReklamLogoVer.BackColor = SystemColors.Control;
        }

        private void btnSondkBant_Click(object sender, EventArgs e)
        {
            if (pnl_TC_Connected.BackColor != Color.LimeGreen) return;

            if (!_isSondkBantOn)
            {
                if (string.IsNullOrWhiteSpace(tbxSondkBant.Text))
                {
                    MessageBox.Show("Lütfen Son Dakika bandı için bir metin giriniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // SET:SONDK BANT komutuyla servise dinamik metni yolluyoruz
                _serviceConnection.SendText($"SET:SONDK BANT:{tbxSondkBant.Text.Trim()}");

                _isSondkBantOn = true;
                btnSondkBant.Text = "SonDk Bant Al";
                btnSondkBant.BackColor = Color.Firebrick;
            }
            else
            {
                // Bandı ekrandan çıkarma komutu
                _serviceConnection.SendText("SONDK BANT AL");

                _isSondkBantOn = false;
                btnSondkBant.Text = "SonDk Bant Ver";
                btnSondkBant.BackColor = Color.Green;
            }
        }
    }
}