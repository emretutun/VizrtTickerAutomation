using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Common;

namespace TickerService
{
    public partial class Form1 : Form
    {
        private SimpleWebServer _webServer;
        private CServer _tcpServer;
        private CVizTicker _hbTicker;
        private VizEngine _vizEngine; // Yeni eklenen motor bağlantımız

        private bool _haberBandiYayinda = false;

        public Form1()
        {
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
                    HaberBandiVer();
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
                        HaberBandiVer();
                        break;

                    case "Haber Ticker Al":
                        _haberBandiYayinda = false;
                        HaberBandiAl();
                        break;
                }
            });
        }

        private void HaberBandiVer()
        {
            List<string> haberler = DataHelper.GetHaberler();

            if (haberler.Count == 0)
            {
                Console.WriteLine("Gönderilecek haber bulunamadı!");
                return;
            }

            // Ticker verisini bas
            _hbTicker.Clear();
            _hbTicker.SendToTicker(haberler.ToArray());

            // Animasyonları tetikle
            _vizEngine.Play("HABER_BASLIK_IN");
            _vizEngine.Play("TICKER_IN");

            Console.WriteLine("Haberler Vizrt Ticker'a gönderildi ve animasyonlar tetiklendi.");
        }

        private void HaberBandiAl()
        {
            _hbTicker.Clear();

            // Bandı ekrandan çıkar
            _vizEngine.Play("TICKER_OUT");

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