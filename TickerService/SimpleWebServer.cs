using System;
using System.Net;
using System.Text;
using System.Threading;
using System.IO;

namespace TickerService
{
    public class SimpleWebServer
    {
        private readonly HttpListener _listener;
        private readonly string _url;
        private Thread _serverThread;

        public SimpleWebServer(string url)
        {
            _url = url;
            _listener = new HttpListener();
            _listener.Prefixes.Add(_url);
        }

        public void Start()
        {
            _serverThread = new Thread(RunServer);
            _serverThread.IsBackground = true; // Servis kapanınca thread'in de kapanmasını sağlar
            _serverThread.Start();
            Console.WriteLine($"Web sunucusu çalışıyor: {_url}");
        }

        private void RunServer()
        {
            _listener.Start();
            try
            {
                while (_listener.IsListening)
                {
                    // HTTP isteğini al
                    var context = _listener.GetContext();
                    ProcessRequest(context);
                }
            }
            catch (HttpListenerException)
            {
                Console.WriteLine("Sunucu durduruldu.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Sunucu hatası: " + ex.Message);
            }
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            string file = request.QueryString["file"];

            // Eğer file parametresi geldiyse dosyanın varlığını kontrol et
            string responseText = "false";
            if (!string.IsNullOrEmpty(file))
            {
                responseText = File.Exists(file) ? "success" : "false";
            }

            byte[] buffer = Encoding.UTF8.GetBytes(responseText);
            response.ContentLength64 = buffer.Length;
            response.ContentType = "text/plain; charset=UTF-8";

            try
            {
                using (var output = response.OutputStream)
                {
                    output.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Yanıt gönderilirken hata oluştu: " + ex.Message);
            }
        }

        public void Stop()
        {
            if (_listener != null && _listener.IsListening)
            {
                _listener.Stop();
                _listener.Close();
            }
            _serverThread?.Join(1000); // Kapanması için en fazla 1 saniye bekle
            Console.WriteLine("Web sunucusu durduruldu.");
        }
    }
}