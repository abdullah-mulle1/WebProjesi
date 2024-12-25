using System;
using System.ComponentModel.DataAnnotations;

namespace Berber.Models
{

    public class Randevu
    {
        public int RandevuId { get; set; }

        public DateTime RndvSt { get; set; } // Randevu Saati
     
        public int CalisanId { get; set; } // Çalışan ID
        public Calisan calisan { get; set; } // Çalışan

       // [Required]
        public int HizmetId { get; set; } // Hizmet ID
        public Hizmet hizmet { get; set; } // Hizmet

        public string UserID { get; set; }
        public Kullanici kullanici { get; set; }

        public string Onay { get; set; } = "Bekliyor"; // Durum: Kullanılıyor mu?
    }
}
