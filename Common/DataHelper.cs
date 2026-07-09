using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Common
{
    // Verinin tipini belirlediğimiz Enum
    public enum TickerDataType
    {
        Haber,
        Skor
    }

    // Vizrt'ye gidecek her bir öğenin (Haber veya Skor) ortak modeli
    public class TickerItem
    {
        public TickerDataType Type { get; set; }
        public string Metin1 { get; set; } // Haber metni VEYA Ev Sahibi Takım
        public string Metin2 { get; set; } // Deplasman Takım (Sadece skor için)
        public string Skor { get; set; }   // Maç Skoru (Sadece skor için)
        public bool CanliMi { get; set; }  // Maç devam ediyor mu?
    }

    public static class DataHelper
    {
        private static readonly string HaberDosyasi = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "haberler.txt");
        private static readonly string SkorDosyasi = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "skorlar.txt");

        // UI'dan gelen "kaç haber, kaç skor" oranına göre listeyi hazırlayan ana metot
        public static List<TickerItem> GetMixedTickerData(int haberSayisi, int skorSayisi)
        {
            var mixedList = new List<TickerItem>();
            var haberler = DosyadanHaberOku();
            var skorlar = DosyadanSkorOku();

            // İstenen sayı kadar haber ekle
            mixedList.AddRange(haberler.Take(haberSayisi));

            // İstenen sayı kadar skor ekle
            mixedList.AddRange(skorlar.Take(skorSayisi));

            return mixedList;
        }

        private static List<TickerItem> DosyadanHaberOku()
        {
            var liste = new List<TickerItem>();
            try
            {
                if (!File.Exists(HaberDosyasi))
                {
                    File.WriteAllText(HaberDosyasi, "Galatasaray, yeni sezon hazırlıklarına başladı.\nFenerbahçe'nin yeni transferi İstanbul'a geldi.\nBeşiktaş, Şampiyonlar Ligi için iddialı.");
                }

                foreach (string satir in File.ReadAllLines(HaberDosyasi))
                {
                    if (!string.IsNullOrWhiteSpace(satir))
                        liste.Add(new TickerItem { Type = TickerDataType.Haber, Metin1 = satir.Trim() });
                }
            }
            catch (Exception ex) { Console.WriteLine("Haber okuma hatası: " + ex.Message); }
            return liste;
        }

        private static List<TickerItem> DosyadanSkorOku()
        {
            var liste = new List<TickerItem>();
            try
            {
                if (!File.Exists(SkorDosyasi))
                {
                    // EvSahibi|Deplasman|Skor|CanliMi formatında örnek veri
                    File.WriteAllText(SkorDosyasi, "Fenerbahçe|Galatasaray|2-1|False\nBeşiktaş|Trabzonspor|0-0|True\nBaşakşehir|Kasımpaşa|1-0|False");
                }

                foreach (string satir in File.ReadAllLines(SkorDosyasi))
                {
                    if (!string.IsNullOrWhiteSpace(satir))
                    {
                        var parcalar = satir.Split('|');
                        if (parcalar.Length >= 3)
                        {
                            liste.Add(new TickerItem
                            {
                                Type = TickerDataType.Skor,
                                Metin1 = parcalar[0].Trim(),
                                Metin2 = parcalar[1].Trim(),
                                Skor = parcalar[2].Trim(),
                                CanliMi = parcalar.Length > 3 && parcalar[3].Trim().ToLower() == "true"
                            });
                        }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("Skor okuma hatası: " + ex.Message); }
            return liste;
        }
    }
}