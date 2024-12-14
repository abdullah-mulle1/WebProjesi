using System;
using System.ComponentModel.DataAnnotations;

namespace Berber.Models
{
    public class Randevu
    {
        public int Id { get; set; }
        [Required]
        public int ClsnId { get; set; } // Çalışan ID
        public Calisan Clsn { get; set; } // Çalışan
        [Required]
        public int HzmtrId { get; set; } // Hizmet ID
        public Hizmet Hzmtr { get; set; } // Hizmet
        [Required]
        public DateTime RndvSt { get; set; } // Randevu Saati
    }
}
