using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berber.Models
{

    public class Randevu
    {
        public int RandevuId { get; set; }

        public string Onay { get; set; } = "Bekliyor"; 
        public string MusteriAd { get; set; }
        public TimeSpan RndvSaat { get; set; } // وقت الموعد

        public DateTime RndvTarih { get; set; }
        public int CalisanId { get; set; } 
        public Calisan calisan { get; set; }

        public int HizmetId { get; set; } 
        public Hizmet hizmet { get; set; }


        public string UserID { get; set; }
        public Kullanici kullanici { get; set; }

    }
}
