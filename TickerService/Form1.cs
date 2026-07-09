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
        private CVizTicker _sdTicker; // Son Dakika Ticker'ı

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

            // Son Dakika Ticker (Veri Portu - 6301)
            _sdTicker = new CVizTicker("sd_scroller", "sd_ticker_text", "sep_sd_htspor");
            bool isSdTickerConnected = _sdTicker.ConnectToTicker();
            Console.WriteLine(isSdTickerConnected ? "SD Ticker'a bağlanıldı." : "SD Ticker bağlantısı BAŞARISIZ!");

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
                    HaberBandiVer(true); // Loop olduğu için true gönderiyoruz
                });
            }
        }

        private void HandleReceivedCommand(CSession client, string msg)
        {
            this.Invoke((MethodInvoker)delegate
            {
                string command = msg.Replace("\0", "").Trim();
                Console.WriteLine($"Ana Kumandadan Gelen Komut: {command}");

                // Eğer komut dinamik bir veri içeriyorsa (Örn: SET:SONDK KJ:Metin)
                if (command.StartsWith("SET:"))
                {
                    // Komutu 3 parçaya ayırıyoruz (SET, KomutTürü, Metin)
                    string[] parts = command.Split(new[] { ':' }, 3);
                    if (parts.Length == 3)
                    {
                        string cmdType = parts[1];
                        string cmdValue = parts[2];

                        if (cmdType == "SONDK KJ")
                        {
                            // Eski kodlarındaki Vizrt objesine metin atama ve animasyon komutları
                            _vizEngine.Send($"RENDERER*MAIN_LAYER*TREE*$SD_TEK_KJ*GEOM*TEXT SET {cmdValue}");
                            _vizEngine.Play("SD_KJ_LOGO");
                            _vizEngine.Play("SD_KJ_TEK_YAZI");
                            Console.WriteLine($"Son Dakika Ekrana Verildi: {cmdValue}");
                        }
                        else if (cmdType == "SONDK BANT")
                        {
                            _sdTicker.Clear();
                            // Common kütüphanesindeki eski SendToTicker(string) metodu hala duruyor olmalı
                            _sdTicker.SendToTicker(cmdValue);
                            _vizEngine.Play("SD_TICKER");
                            Console.WriteLine($"Son Dakika Bandı Ekrana Verildi: {cmdValue}");
                        }
                    }
                    return; // SET işlemini hallettik, switch-case'e girmesine gerek yok
                }

                // Sabit komutlar
                switch (command)
                {
                    case "Haber Ticker Ver":
                        _haberBandiYayinda = true;
                        HaberBandiVer(false);
                        break;

                    case "Haber Ticker Al":
                        _haberBandiYayinda = false;
                        HaberBandiAl();
                        break;

                    case "SONDK KJ AL":
                        _vizEngine.Play("SD_KJ_OUT");
                        Console.WriteLine("Son Dakika Yayından Alındı.");
                        break;

                    case "CANLI VER":
                        _vizEngine.Send("RENDERER*MAIN_LAYER*TREE*$CANLI_YAZI*GEOM*TEXT SET CANLI");
                        _vizEngine.Play("CANLI");
                        Console.WriteLine("CANLI Logosu Verildi.");
                        break;

                    case "TEKRAR VER":
                        _vizEngine.Send("RENDERER*MAIN_LAYER*TREE*$CANLI_YAZI*GEOM*TEXT SET TEKRAR");
                        _vizEngine.Play("CANLI");
                        Console.WriteLine("TEKRAR Logosu Verildi.");
                        break;

                    case "CANLI AL":
                        _vizEngine.Play("CANLI_OUT");
                        Console.WriteLine("Canlı/Tekrar Logosu Alındı.");
                        break;
                    case "LOGO VER":
                        _vizEngine.Play("LOGO");
                        Console.WriteLine("Normal Logo Ekrana Verildi.");
                        break;

                    case "REKLAM LOGO VER":
                        _vizEngine.Play("LOGO_REKLAM");
                        Console.WriteLine("Reklam Logosu Ekrana Verildi.");
                        break;

                    case "LOGO AL":
                        // Ekranda hangisi varsa çıksın diye her iki OUT animasyonunu da yolluyoruz
                        _vizEngine.Play("LOGO_OUT");
                        _vizEngine.Play("LOGO_REKLAM_OUT");
                        Console.WriteLine("Logolar Yayından Alındı.");
                        break;
                    case "SONDK BANT AL":
                        _vizEngine.Play("SD_TICKER_OUT");
                        Console.WriteLine("Son Dakika Bandı Yayından Alındı.");
                        break;
                }
            });
        }

        // isLoop parametresi sayesinde, veri başa sardığında (loop) tekrar IN animasyonu oynamayacak
        private void HaberBandiVer(bool isLoop = false)
        {
            List<TickerItem> veriler = DataHelper.GetMixedTickerData(3, 10);

            if (veriler.Count == 0) return;

            _hbTicker.Clear();
            _hbTicker.SendToTicker(veriler);

            // Eğer butona yeni basıldıysa (ilk girişse) animasyonu oynat
            if (!isLoop)
            {
                _vizEngine.Play("HABER_BASLIK_IN");
                _vizEngine.Play("TICKER_IN");
                Console.WriteLine("Veriler gönderildi ve IN animasyonu tetiklendi.");
            }
        }

        private void HaberBandiAl()
        {
            // Grafiğin içindeki yazılar silinmeden pürüzsüzce ekrandan çıkması gerekiyor.
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