using Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TickerService
{
    public partial class Form1 : Form
    {
        private SimpleWebServer _webServer;
        private CServer _tcpServer;
        private CVizTicker _hbTicker;
        private VizEngine _vizEngine; // Yeni eklenen motor bağlantımız

        private bool _haberBandiYayinda = false;
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
        public Form1()
        {
            AllocConsole();
            InitializeComponent();
            this.Load += Form1_Load;
            this.FormClosing += Form1_FormClosing;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // 1. Web Sunucusunu Başlat
            _webServer = new SimpleWebServer("http://+:8090/");
            _webServer.Start();

            // 2. TCP Sunucusunu Başlat
            _tcpServer = new CServer();
            _tcpServer.OnListenBegin += port => Console.WriteLine($"TCP Server {port} portunda dinliyor.");
            _tcpServer.OnReceivedClient += HandleReceivedCommand;
            _tcpServer.BeginListen(6405);

            // 3. Viz Ticker (Veri Portu - 6301)
            _hbTicker = new CVizTicker("haber_scroller", "haber_ticker_text", "sep_haber_htspor");
            _hbTicker.VizTickerFinished += HbTicker_VizTickerFinished;
            bool isTickerConnected = _hbTicker.ConnectToTicker();
            Console.WriteLine(isTickerConnected ? "Vizrt Ticker'a bağlanıldı." : "Vizrt Ticker bağlantısı BAŞARISIZ!");

            // 4. Viz Engine (Animasyon Portu - 6100)
            _vizEngine = new VizEngine("127.0.0.1", 6100);
            bool isEngineConnected = _vizEngine.Connect();
            Console.WriteLine(isEngineConnected ? "Viz Engine'e bağlanıldı." : "Viz Engine bağlantısı BAŞARISIZ!");
        }

        private void HbTicker_VizTickerFinished(string tickerName)
        {
            if (_haberBandiYayinda)
            {
                this.Invoke((MethodInvoker)delegate {
                    // Loop (Döngü) olduğu için true gönderiyoruz. IN animasyonu tetiklenmeyecek.
                    HaberBandiVer(true);
                });
            }
        }

        private void HandleReceivedCommand(CSession client, string msg)
        {
            this.Invoke((MethodInvoker)delegate
            {
                string command = msg.Replace("\0", "").Trim();
                Console.WriteLine($"Ana Kumandadan Gelen Komut: {command}");

                switch (command)
                {
                    case "Haber Ticker Ver":
                        _haberBandiYayinda = true;
                        HaberBandiVer(false); // Butona basıldığı (ilk giriş) için false
                        break;

                    case "Haber Ticker Al":
                        _haberBandiYayinda = false;
                        HaberBandiAl();
                        break;
                }
            });
        }

        private void HaberBandiVer(bool isLoop = false)
        {
            // 3 Haber, 10 Skor kuralını işletiyoruz
            List<TickerItem> veriler = DataHelper.GetMixedTickerData(3, 10);

            if (veriler.Count == 0)
            {
                Console.WriteLine("Gönderilecek veri bulunamadı!");
                return;
            }

            // Ticker verisini bas
            _hbTicker.Clear();
            _hbTicker.SendToTicker(veriler);

            // Eğer bu bir döngü (loop) değilse, yani butona yeni basıldıysa animasyonları oynat
            if (!isLoop)
            {
                _vizEngine.Play("HABER_BASLIK_IN");
                _vizEngine.Play("TICKER_IN");
                Console.WriteLine($"{veriler.Count} adet karma veri Vizrt Ticker'a gönderildi ve animasyonlar tetiklendi.");
            }
            else
            {
                Console.WriteLine("Veriler bitti, animasyonsuz olarak yeni tura (loop) geçildi.");
            }
        }

        private void HaberBandiAl()
        {
            _hbTicker.Clear();

            // Bandı ekrandan çıkar
            //_vizEngine.Play("TICKER_OUT");

            Console.WriteLine("Haber bandı yayından alındı.");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _webServer?.Stop();
            _tcpServer?.StopListen();
            _vizEngine?.Disconnect();
            _hbTicker?.Clear();
        }
    }
}