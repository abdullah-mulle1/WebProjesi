using System.ComponentModel.DataAnnotations.Schema;

namespace Berber.Models
{
    public class CalisanHizmet
    {
        public int CalisanId { get; set; } // المفتاح الأجنبي الخاص بـ Calisan
        public Calisan calisan { get; set; } // العلاقة مع كيان Calisan

        public int HizmetId { get; set; } // المفتاح الأجنبي الخاص بـ Uzmanlik
        public Hizmet hizmet { get; set; } // العلاقة مع كيان Uzmanlik
    }
}
