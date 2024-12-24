using Berber.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Berber.Data
{
    public static class KullaniciRolu
    {
        public static async Task InitializeAsync(IServiceProvider hizmetSaglayici)
        {
            var rolYoneticisi = hizmetSaglayici.GetRequiredService<RoleManager<IdentityRole>>();
            var kullaniciYoneticisi = hizmetSaglayici.GetRequiredService<UserManager<Kullanici>>();

            string[] rolIsimleri = { "Admin", "Calisan", "Kullanici" };
            IdentityResult rolSonucu;

            foreach (var rol in rolIsimleri)
            {
                var rolVarMi = await rolYoneticisi.RoleExistsAsync(rol);
                if (!rolVarMi)
                {
                    rolSonucu = await rolYoneticisi.CreateAsync(new IdentityRole(rol));
                }
            }

            var adminEposta = "b211210577@sakarya.edu.tr";
            var adminSifre = "sau";


            // Kontrol: Admin kullanıcısı zaten var mı?
            if (await kullaniciYoneticisi.FindByEmailAsync(adminEposta) == null)
            {
                var adminKullanici = new Kullanici
                {
                    Email = adminEposta,
                    UserName = adminSifre,
                     EmailConfirmed = true
                };

                var sonuc = await kullaniciYoneticisi.CreateAsync(adminKullanici, adminSifre);
                if (sonuc.Succeeded)
                {
                    // Admin rolü ata
                    await kullaniciYoneticisi.AddToRoleAsync(adminKullanici, "Admin");
                }
            }
        }
    }
}

