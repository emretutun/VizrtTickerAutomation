using System;
using System.Net.Sockets;
using System.Text;

namespace Common
{
    public class VizEngine
    {
        private TcpClient _client;
        private NetworkStream _stream;
        public string IP { get; private set; }
        public int Port { get; private set; }
        public bool IsConnected { get; private set; }

        public VizEngine(string ip = "127.0.0.1", int port = 6100)
        {
            IP = ip;
            Port = port;
        }

        // Vizrt Engine'e bağlanır
        public bool Connect()
        {
            try
            {
                if (_client != null && _client.Connected) return true;

                _client = new TcpClient();
                _client.Connect(IP, Port);
                _stream = _client.GetStream();
                IsConnected = true;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Vizrt Engine ({IP}:{Port}) Bağlantı Hatası: {ex.Message}");
                IsConnected = false;
                return false;
            }
        }

        // Engine ile bağlantıyı keser
        public void Disconnect()
        {
            IsConnected = false;
            _stream?.Close();
            _client?.Close();
        }

        // Engine'e ham komut gönderir (Eski kodundaki -1 ve \0 mantığıyla çalışır)
        public void Send(string command)
        {
            if (!IsConnected)
            {
                // Bağlantı koptuysa yeniden bağlanmayı dene
                if (!Connect()) return;
            }

            try
            {
                // Viz Engine raw komutlarında cevap beklemediğimiz durumlar için -1 kullanılır.
                string fullCommand = "-1 " + command + "\0";
                byte[] data = Encoding.UTF8.GetBytes(fullCommand);
                _stream.Write(data, 0, data.Length);
            }
            catch
            {
                IsConnected = false;
            }
        }

        // Eski kodundaki gibi kolayca animasyon tetiklemek için yardımcı metot
        public void Play(string animName, string layer = "MAIN_LAYER")
        {
            // O anki sahnenin Stage Director'ünü başlatır
            Send($"RENDERER*{layer}*STAGE*DIRECTOR*{animName} START");
        }

        public void ReversePlay(string animName, string layer = "MAIN_LAYER")
        {
            Send($"RENDERER*{layer}*STAGE*DIRECTOR*{animName} START REVERSE");
        }
    }
}