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

            // Çalışan-Hizmet Çoktan Çoğa İlişkisi
            modelBuilder.Entity<CalisanHizmet>()
                .HasKey(ch => new { ch.CalisanId, ch.HizmetId }); // Birleşik birincil anahtar

            modelBuilder.Entity<CalisanHizmet>()
                .HasOne(ch => ch.calisan)
                .WithMany(c => c.Hizmetler)
                .HasForeignKey(ch => ch.CalisanId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CalisanHizmet>()
                .HasOne(ch => ch.hizmet)
                .WithMany(h => h.Calisanlar)
                .HasForeignKey(ch => ch.HizmetId)
                .OnDelete(DeleteBehavior.Cascade);

            // Randevu İlişkileri
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.calisan)
                .WithMany()
                .HasForeignKey(r => r.CalisanId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.hizmet)
                .WithMany()
                .HasForeignKey(r => r.HizmetId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.kullanici)
                .WithMany()
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Restrict);

 
        }

    }
}
