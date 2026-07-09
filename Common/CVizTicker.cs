using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using VIZTICKERLib;

namespace Common
{
    public class CVizTicker
    {
        #region Definitions
        public delegate void tickerCallback(string transaction, string state2, string key, int ttl);
        public delegate void tickerFinished(string tickerName);
        public event tickerCallback VizTickerCallBack;
        public event tickerFinished VizTickerFinished;
        IVizTickers tickers;
        IVizTickerControl tickerControl;
        public int ttl = 1;
        public string TickerName = "";
        public bool Seperator = true;
        public string TextObject;
        public string SeperatorObject;

        public CVizTicker() { }

        public CVizTicker(string _tickerName, string _TextObject, string _SeperatorObject, bool _Seperator = true)
        {
            TickerName = _tickerName;
            Seperator = _Seperator;
            TextObject = _TextObject;
            SeperatorObject = _SeperatorObject;
        }
        #endregion

        #region Connection
        public bool ConnectToTicker(string _tickerName)
        {
            TickerName = _tickerName;
            return ConnectToTicker();
        }

        public bool ConnectToTicker()
        {
            try
            {
                tickers = new VizTickers();
            }
            catch (Exception e)
            {
                string hata = e.ToString();
                MessageBox.Show("Hata Kodu: '" + hata + "'");
                return false;
            }
            if (tickers.GetTicker(TickerName) == null)
                tickers.AddTicker(TickerName);

            tickerControl = tickers.GetTicker(TickerName);
            ConnectionControlToTickerConsole();
            return true;
        }
        #endregion

        #region Data Send
        private string ElementXML(string groupName, int key, string template, string text = null)
        {
            string elementXML = "<element key=\"" + key + "\">"
                              + "<design>" + template + "</design>"
                              + "<ttl>" + ttl + "</ttl>"
                              + ((text != null) ? "<value label=\"" + template + "\" attribute=\"text\">" + text + "</value>" : "")
                              + "</element>";
            return elementXML;
        }

        int groupNameCounter = 0;
        string previousGroup = "";

        public int SendToTicker(string text)
        {
            groupNameCounter++;
            string groupName = (groupNameCounter).ToString();
            if (groupNameCounter > 100) groupNameCounter = 0;

            tickerControl.DeleteGroup(groupName);
            string xml = "<group name=\"" + groupName + "\">";
            xml += ElementXML(groupName, 1, TextObject, text);
            xml += (SeperatorObject != string.Empty) ? ElementXML(groupName, 2, SeperatorObject) : "";
            xml += "</group>";

            tickerControl.AddGroupAfterGroup(previousGroup, xml);
            previousGroup = groupName;
            return groupNameCounter;
        }

        public string ConvertStupidEncoding(string xml)
        {
            return xml.Replace("Ç", "├Ç")
                      .Replace("ç", "├ç")
                      .Replace("Ö", "├Ö")
                      .Replace("ö", "├ö")
                      .Replace("ı", "├ı")
                      .Replace("İ", "├İ")
                      .Replace("Ğ", "├Ğ")
                      .Replace("ğ", "├ğ")
                      .Replace("Ş", "├Ş")
                      .Replace("Ü", "├Ü")
                      .Replace("ü", "├ü")
                      .Replace("ø", "├ø")
                      .Replace("Ø", "├Ø")
                      .Replace("Å", "├Å")
                      .Replace("å", "├å")
                      .Replace("Ä", "├Ä")
                      .Replace("ä", "├ä")
                      .Replace("Æ", "├Æ")
                      .Replace("æ", "├æ")
                      .Replace("Î", "├Î")
                      .Replace("&", "&amp;");
        }

        public int SendToTicker(List<TickerItem> items)
        {
            int ret = 0;
            foreach (var item in items)
            {
                groupNameCounter++;
                string groupName = groupNameCounter.ToString();
                if (groupNameCounter > 100) groupNameCounter = 0;

                tickerControl.DeleteGroup(groupName);
                string xml = $"<group name=\"{groupName}\">";

                if (item.Type == TickerDataType.Haber)
                {
                    // Haber ise standart haber XML'i oluştur
                    xml += ElementXML(groupName, 1, "haber_ticker_text", ConvertStupidEncoding(item.Metin1));
                    xml += ElementXML(groupName, 2, "sep_haber_htspor");
                }
                else if (item.Type == TickerDataType.Skor)
                {
                    // Skor ise skor_group tasarımına uygun XML oluştur
                    string evSahibi = ConvertStupidEncoding(item.Metin1);
                    string deplasman = ConvertStupidEncoding(item.Metin2);
                    string skorText = ConvertStupidEncoding(item.Skor);

                    string values = $"<value label=\"takim_ev\" attribute=\"text\">{evSahibi}</value>" +
                                    $"<value label=\"takim_deplasman\" attribute=\"text\">{deplasman}</value>";

                    if (item.CanliMi)
                    {
                        values += $"<value label=\"skor_canli\" attribute=\"text\">{skorText}</value>" +
                                  $"<value label=\"skor_mac_sonu\" attribute=\"text\"> </value>";
                    }
                    else
                    {
                        values += $"<value label=\"skor_mac_sonu\" attribute=\"text\">{skorText}</value>" +
                                  $"<value label=\"skor_canli\" attribute=\"text\"> </value>";
                    }

                    xml += $"<element key=\"1\"><design>skor_group</design><ttl>{ttl}</ttl>{values}</element>";
                    xml += ElementXML(groupName, 2, "sep_skor_htspor"); // Skor için ayırıcı
                }

                xml += "</group>";
                tickerControl.AddGroupAfterGroup(previousGroup, xml);
                previousGroup = groupName;
                ret = groupNameCounter;
            }
            return ret;
        }

        public void Clear()
        {
            tickerControl.ClearAll();
            groupNameCounter = 0;
            previousGroup = "";
        }
        #endregion

        #region ticker listener
        private TcpClient tcpTickerPort;
        private static NetworkStream tcr_Stream;
        private static byte[] tcr_Buffer = new byte[1024];
        string _engineIp = "127.0.0.1";
        int _port = 6301;

        public void ConnectionControlToTickerConsole()
        {
            if (tcpTickerPort == null)
            {
                tcpTickerPort = new TcpClient();
                try
                {
                    tcpTickerPort.Connect(_engineIp, _port);
                }
                catch
                {
                    tcpTickerPort.Close();
                    return;
                }
            }
            if (tcpTickerPort.Connected)
            {
                tcr_Stream = tcpTickerPort.GetStream();
                tcr_Stream.BeginRead(tcr_Buffer, 0, tcr_Buffer.Length, new AsyncCallback(OnTickerEventReceived), tcr_Stream);
                SendCommandToConsole("* protocol tickertalk");
            }
        }

        private bool SendCommandToConsole(string text)
        {
            text += "\n";
            try
            {
                System.IO.Stream tmpStream = tcpTickerPort.GetStream();
                UTF8Encoding asEn = new UTF8Encoding();
                byte[] byte_Array = System.Text.Encoding.GetEncoding("ISO-8859-9").GetBytes(text);
                tmpStream.Write(byte_Array, 0, byte_Array.Length);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void ReadTickerCallbacks(String aData)
        {
            if (aData.Trim() != "")
            {
                Console.WriteLine(TickerName + " : " + aData);
                string[] sWord = aData.Split(' ');
                int txtCount = sWord.Count();
                string transaction = "";
                string key = "";
                int ttl = 0;
                string state2 = "";
                string tickerName = sWord[3].Trim();

                if (txtCount == 4)
                {
                    transaction = sWord[1];
                    if (transaction == "run" & sWord[2].Contains("?TTL="))
                    {
                        string[] runPac = sWord[2].Split('?');
                        key = runPac[0];
                        ttl = Int16.Parse(runPac[1].Split('=')[1]);
                    }
                    else
                        state2 = sWord[2];

                    if (VizTickerCallBack != null) VizTickerCallBack.Invoke(transaction.Trim(), state2.Trim(), key.Trim(), ttl);
                    if (VizTickerFinished != null) if (state2.Equals("SCROLLER_EMPTY")) VizTickerFinished.Invoke(TickerName);
                }
                else
                    Console.WriteLine("Tanımsız ticker Callback mesajı:" + aData);
            }
        }

        public void OnTickerEventReceived(IAsyncResult asyn)
        {
            try
            {
                int iRx = tcr_Stream.EndRead(asyn);
                ReadTickerCallbacks(Encoding.UTF8.GetString(tcr_Buffer, 0, iRx - 1));
                tcr_Stream.BeginRead(tcr_Buffer, 0, tcr_Buffer.Length, new AsyncCallback(OnTickerEventReceived), tcr_Stream);
                tcr_Stream.Flush();
            }
            catch (Exception) { }
        }
        #endregion
    }
}