using System.ComponentModel.DataAnnotations;

namespace Berber.Models
{
    public class Hizmet

    {
        public int HizmetId { get; set; }
        [Required]
        public string Ad { get; set; } // Hizmet Adı
        [Required]
        public decimal Ucrt { get; set; } // Ücret
        public int SrDk { get; set; } // Süre (dakika)

        public ICollection<CalisanHizmet> Calisanlar { get; set; } = new List<CalisanHizmet>();
    }
}
