using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Berber.Models
{
    public class BerberContext : IdentityDbContext<Kullanici>
    {
        public BerberContext(DbContextOptions<BerberContext> options) : base(options) { }

        public DbSet<Calisan> Clsnlr { get; set; } // Çalışanlar
        public DbSet<Hizmet> Hzmtrlr { get; set; } // Hizmetler
        public DbSet<Randevu> Rndvrlr { get; set; } // Randevular
    }
}
