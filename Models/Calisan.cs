using System.ComponentModel.DataAnnotations;

namespace Berber.Models
{
    public class Calisan
    {
        public int CalisanId { get; set; }

        [Required]
        public string Ad { get; set; } // اسم الموظف

        [Required]
        public TimeSpan SaatBaslangic { get; set; } // وقت البداية

        [Required]
        public TimeSpan SaatBitis { get; set; } // وقت النهاية

        public ICollection<CalisanHizmet> Hizmetler { get; set; } = new List<CalisanHizmet>();
    }
}
