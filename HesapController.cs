using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Berber.Models;
using Berber.ViewModels;

namespace Berber.Controllers
{
    public class HesapController : Controller
    {
        private readonly UserManager<Kullanici> _kullaniciYoneticisi;
        private readonly SignInManager<Kullanici> _girisYoneticisi;

        public HesapController(UserManager<Kullanici> kullaniciYoneticisi, SignInManager<Kullanici> girisYoneticisi)
        {
            _kullaniciYoneticisi = kullaniciYoneticisi;
            _girisYoneticisi = girisYoneticisi;
        }

        [HttpGet]
        public IActionResult KayitOl()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> KayitOl(KayitOlViewModel model)
        {
            if (ModelState.IsValid)
            {
                var kullanici = new Kullanici { UserName = model.KullaniciAdi, Email = model.Eposta, Ad = model.Ad, Soyad = model.Soyad };
                var sonuc = await _kullaniciYoneticisi.CreateAsync(kullanici, model.Parola);

                if (sonuc.Succeeded)
                {
                    await _girisYoneticisi.SignInAsync(kullanici, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var hata in sonuc.Errors)
                {
                    ModelState.AddModelError(string.Empty, hata.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult GirisYap()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GirisYap(GirisYapViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sonuc = await _girisYoneticisi.PasswordSignInAsync(model.KullaniciAdi, model.Parola, model.BeniHatirla, false);

                if (sonuc.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CikisYap()
        {
            await _girisYoneticisi.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
