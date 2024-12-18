using System.ComponentModel.DataAnnotations;

namespace Berber.Models
{
    public class Uzmanlik
    {
        public int Id { get; set; } // المفتاح الأساسي

        [Required]
        public string Ad { get; set; } // اسم التخصص

        public int CalisanId { get; set; } // المفتاح الأجنبي
        public Calisan Calisan { get; set; } // العلاقة مع كيان Calisan
    }
}
