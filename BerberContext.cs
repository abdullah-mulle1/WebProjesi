using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Berber.Models
{
    public class BerberContext : IdentityDbContext<Kullanici>
    {
        public DbSet<Calisan> Calisanlar { get; set; } // جدول الموظفين
        public DbSet<Hizmet> Hizmetler { get; set; } // جدول الخدمات
        public DbSet<CalisanHizmet> CalisanHizmetler { get; set; } // جدول العلاقة بين الموظفين والتخصصات
        public DbSet<Randevu> Randevular { get; set; } // جدول المواعيد

        public BerberContext(DbContextOptions<BerberContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.kullanici)
                .WithMany()
                .HasForeignKey(r => r.UserID);


            // Çalışan-Hizmet Çoktan Çoğa İlişkisi
            modelBuilder.Entity<CalisanHizmet>()
                .HasKey(ch => new { ch.CalisanId, ch.HizmetId }); // Birleşik birincil anahtar

            modelBuilder.Entity<CalisanHizmet>()
                .HasOne(ch => ch.calisan)
                .WithMany(c => c.CalisanHizmetler)
                .HasForeignKey(ch => ch.CalisanId);

            modelBuilder.Entity<CalisanHizmet>()
                .HasOne(ch => ch.hizmet)
                .WithMany(h => h.Calisanlar)
                .HasForeignKey(ch => ch.HizmetId);

            
            // Relationship between Kullanici and Randevu


        }

    }
}
