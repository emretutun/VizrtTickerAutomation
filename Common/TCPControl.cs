using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Common
{
    // --- Event Tanımlamaları ---
    public delegate void OnServerListenBeginEvent(int port);
    public delegate void OnServerListenBeginErrorEvent(int port, string message);
    public delegate void OnServerConnectClientEvent(CSession handle);
    public delegate void OnServerReceivedClientEvent(CSession client, string msg);

    public delegate void OnSessionConnectEvent(CSession client);
    public delegate void OnSessionDisconnectEvent(CSession client);
    public delegate void OnSessionReceivedEvent(CSession client, string msg);

    // --- İstemci (Client) Oturum Sınıfı ---
    public class CSession
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private Thread _receiveThread;
        private bool _isConnected;

        public string ID { get; private set; }
        public string IP { get; private set; }

        public event OnSessionConnectEvent OnConnect;
        public event OnSessionDisconnectEvent OnDisconnect;
        public event OnSessionReceivedEvent OnReceived;

        // Server tarafında bir istemci bağlandığında çalışacak yapıcı metot
        public CSession(TcpClient client)
        {
            _client = client;
            ID = Guid.NewGuid().ToString().Substring(0, 8); // Kısa ve benzersiz ID
            IP = ((IPEndPoint)_client.Client.RemoteEndPoint).Address.ToString();
            StartReceiving();
        }

        // UI (Ana Kumanda) tarafında servise bağlanırken çalışacak yapıcı metot
        public CSession(string ip, int port)
        {
            IP = ip;
            _client = new TcpClient();
        }

        // Manuel bağlantı tetikleyici (UI için)
        public void Connect(int port = 6405)
        {
            try
            {
                if (!_client.Connected)
                {
                    _client.Connect(IP, port);
                    StartReceiving();
                    OnConnect?.Invoke(this);
                }
            }
            catch
            {
                _isConnected = false;
                // İleride buraya loglama eklenebilir
            }
        }

        private void StartReceiving()
        {
            _isConnected = true;
            _stream = _client.GetStream();
            _receiveThread = new Thread(ReceiveLoop);
            _receiveThread.IsBackground = true;
            _receiveThread.Start();
        }

        private void ReceiveLoop()
        {
            byte[] buffer = new byte[4096];
            try
            {
                while (_isConnected && _client.Connected)
                {
                    int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; // Karşı taraf bağlantıyı kopardı

                    string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    OnReceived?.Invoke(this, msg);
                }
            }
            catch { }
            finally
            {
                Disconnect();
            }
        }

        public void SendText(string msg)
        {
            if (!_isConnected || _stream == null) return;
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(msg);
                _stream.Write(data, 0, data.Length);
            }
            catch
            {
                Disconnect();
            }
        }

        public void Disconnect()
        {
            if (!_isConnected) return;
            _isConnected = false;
            _stream?.Close();
            _client?.Close();
            OnDisconnect?.Invoke(this);
        }
    }

    // --- Sunucu (Server) Sınıfı ---
    public class CServer
    {
        private TcpListener _listener;
        private Thread _listenThread;
        private bool _isListening;

        public Dictionary<string, CSession> ClientSessions { get; private set; } = new Dictionary<string, CSession>();

        public event OnServerListenBeginEvent OnListenBegin;
        public event OnServerListenBeginErrorEvent OnListenBeginError;
        public event OnServerConnectClientEvent OnConnectClient;
        public event OnServerReceivedClientEvent OnReceivedClient;

        public void BeginListen(int port)
        {
            try
            {
                _listener = new TcpListener(IPAddress.Any, port);
                _listener.Start();
                _isListening = true;

                _listenThread = new Thread(AcceptClientsLoop);
                _listenThread.IsBackground = true;
                _listenThread.Start();

                OnListenBegin?.Invoke(port);
            }
            catch (Exception ex)
            {
                OnListenBeginError?.Invoke(port, ex.Message);
            }
        }

        private void AcceptClientsLoop()
        {
            while (_isListening)
            {
                try
                {
                    TcpClient tcpClient = _listener.AcceptTcpClient();
                    CSession session = new CSession(tcpClient);

                    ClientSessions[session.ID] = session;

                    // Client'tan mesaj geldiğinde server'ın eventini tetikle
                    session.OnReceived += (c, msg) => OnReceivedClient?.Invoke(c, msg);

                    // Client koptuğunda listeden çıkar
                    session.OnDisconnect += (c) => ClientSessions.Remove(c.ID);

                    OnConnectClient?.Invoke(session);
                }
                catch
                {
                    if (!_isListening) break;
                }
            }
        }

        public void StopListen()
        {
            _isListening = false;
            _listener?.Stop();

            // Tüm bağlı istemcileri güvenlice kapat
            foreach (var session in ClientSessions.Values)
            {
                session.Disconnect();
            }
            ClientSessions.Clear();
        }
    }
}