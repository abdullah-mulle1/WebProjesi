using System.ComponentModel.DataAnnotations;

namespace Berber.Models
{
    public class Calisan
    {
        public int Id { get; set; }
        [Required]
        public string Ad { get; set; } // Adı
        [Required]
        public string Uzmnlk { get; set; } // Uzmanlık Alanı
        public bool Mst { get; set; } // Müsaitlik
    }
}
