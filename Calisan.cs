using Berber.Models;
using System.ComponentModel.DataAnnotations;

public class Calisan
{
    // Primary Key
    public int CalisanId { get; set; }

    [Required]
    public string Ad { get; set; } // Name

    [Required]
    public TimeSpan SaatBaslangic { get; set; } // Start Time

    [Required]
    public TimeSpan SaatBitis { get; set; } // End Time

    public string? UserID { get; set; } // Link to AspNetUsers table

    // Relationships
    public ICollection<CalisanHizmet> CalisanHizmetler { get; set; } = new List<CalisanHizmet>();
    public ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
}
