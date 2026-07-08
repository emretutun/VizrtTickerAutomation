using System;
using System.Collections.Generic;
using System.IO;

namespace Common
{
    public static class DataHelper
    {
        // Uygulamanın çalıştığı dizindeki haberler.txt dosyasının yolu
        private static readonly string HaberDosyasi = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "haberler.txt");
        private static readonly string SonDakikaDosyasi = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sondakika.txt");

        // Haberleri dosyadan okuyup liste olarak döndüren metot
        public static List<string> GetHaberler()
        {
            return DosyadanOku(HaberDosyasi);
        }

        // Son dakika verilerini dosyadan okuyup liste olarak döndüren metot
        public static List<string> GetSonDakika()
        {
            return DosyadanOku(SonDakikaDosyasi);
        }

        // Genel dosya okuma metodu
        private static List<string> DosyadanOku(string dosyaYolu)
        {
            var liste = new List<string>();
            try
            {
                if (File.Exists(dosyaYolu))
                {
                    string[] satirlar = File.ReadAllLines(dosyaYolu);
                    foreach (string satir in satirlar)
                    {
                        // Sadece boş olmayan satırları listeye ekle
                        if (!string.IsNullOrWhiteSpace(satir))
                        {
                            liste.Add(satir.Trim());
                        }
                    }
                }
                else
                {
                    // Dosya yoksa varsayılan bir veri oluştur ki boş kalmasın
                    File.WriteAllText(dosyaYolu, "Örnek Haber 1\nÖrnek Haber 2\nÖrnek Haber 3");
                    liste.Add("Örnek Haber 1");
                    liste.Add("Örnek Haber 2");
                    liste.Add("Örnek Haber 3");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Dosya okuma hatası ({dosyaYolu}): {ex.Message}");
            }

            return liste;
        }
    }
}