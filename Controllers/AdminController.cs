using Microsoft.AspNetCore.Mvc;
using Berber.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Berber.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly BerberContext _context;

        public AdminController(BerberContext context)
        {
            _context = context;
        }

        public IActionResult Index ()
        {
            return View();
        }
        public IActionResult Calisanlar()
        {
            var calisanlar = _context.Calisanlar
                .Include(c => c.Hizmetler) // Load the CalisanHizmet relationship
                .ThenInclude(ch => ch.hizmet) // Load the Hizmet details
                .ToList();

            return View(calisanlar);
        }

        // Çalışan Ekleme (GET)
        public IActionResult CalisanEkle()
        {
            var hizmetler = _context.Hizmetler.ToList();
            ViewBag.Hizmetler = hizmetler; // Pass hizmetler to the view
            return View(new Calisan());
        }


        // Çalışan Ekleme (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CalisanEkle(Calisan calisan, int[] selectedHizmetler)
        {
            if (ModelState.IsValid)
            {
                // Add new employee
                _context.Calisanlar.Add(calisan);
                _context.SaveChanges();

                // Add selected hizmetler to the CalisanHizmet table
                foreach (var hizmetId in selectedHizmetler)
                {
                    _context.CalisanHizmetler.Add(new CalisanHizmet
                    {
                        CalisanId = calisan.CalisanId,
                        HizmetId = hizmetId
                    });
                }

                _context.SaveChanges();
                return RedirectToAction("Calisanlar");
            }

            // Re-fetch hizmetler in case of validation error
            ViewBag.Hizmetler = _context.Hizmetler.ToList();
            return View(calisan);
        }


        public IActionResult CalisanGuncelle(int id)
        {
            var calisan = _context.Calisanlar
                .Include(c => c.Hizmetler)
                .ThenInclude(ch => ch.hizmet)
                .FirstOrDefault(c => c.CalisanId == id);

            if (calisan == null)
            {
                return NotFound();
            }

            ViewBag.Hizmetler = _context.Hizmetler.ToList(); // Pass all hizmetler to the view
            return View(calisan);
        }

        // Çalışan Güncelleme (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CalisanGuncelle(Calisan calisan, int[] SelectedHizmetler)
        {
            if (ModelState.IsValid)
            {
                // Update employee details
                var existingCalisan = _context.Calisanlar
                    .Include(c => c.Hizmetler)
                    .FirstOrDefault(c => c.CalisanId == calisan.CalisanId);

                if (existingCalisan == null)
                {
                    return NotFound();
                }

                existingCalisan.Ad = calisan.Ad;
                existingCalisan.SaatBaslangic = calisan.SaatBaslangic;
                existingCalisan.SaatBitis = calisan.SaatBitis;

                // Update Hizmetler
                existingCalisan.Hizmetler.Clear(); // Remove existing relationships
                foreach (var hizmetId in SelectedHizmetler)
                {
                    existingCalisan.Hizmetler.Add(new CalisanHizmet
                    {
                        CalisanId = calisan.CalisanId,
                        HizmetId = hizmetId
                    });
                }

                _context.SaveChanges();
                return RedirectToAction("Calisanlar");
            }

            ViewBag.Hizmetler = _context.Hizmetler.ToList(); // Reload hizmetler in case of validation error
            return View(calisan);
        }


        // Çalışan Silme
        public IActionResult CalisanSil(int id)
        {
            var calisan = _context.Calisanlar.Find(id);
            if (calisan != null)
            {
                _context.Calisanlar.Remove(calisan);
                _context.SaveChanges();
            }
            return RedirectToAction("Calisanlar");
        }

        // Hizmetler CRUD İşlemleri
        // Listeleme
        public IActionResult Hizmetler()
        {
            var hizmetler = _context.Hizmetler.ToList();
            return View(hizmetler);
        }

        // Hizmet Ekleme (GET)
        public IActionResult HizmetEkle()
        {
            return View();
        }

        // Hizmet Ekleme (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HizmetEkle(Hizmet hizmet)
        {
            if (ModelState.IsValid)
            {
                _context.Hizmetler.Add(hizmet);
                _context.SaveChanges();
                return RedirectToAction("Hizmetler");
            }
            return View(hizmet);
        }

        // Hizmet Güncelleme (GET)
        public IActionResult HizmetGuncelle(int id)
        {
            var hizmet = _context.Hizmetler.Find(id);
            if (hizmet == null)
            {
                return NotFound();
            }
            return View(hizmet);
        }

        // Hizmet Güncelleme (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HizmetGuncelle(Hizmet hizmet)
        {
            if (ModelState.IsValid)
            {
                _context.Hizmetler.Update(hizmet);
                _context.SaveChanges();
                return RedirectToAction("Hizmetler");
            }
            return View(hizmet);
        }

        // Hizmet Silme
        public IActionResult HizmetSil(int id)
        {
            var hizmet = _context.Hizmetler.Find(id);
            if (hizmet != null)
            {
                _context.Hizmetler.Remove(hizmet);
                _context.SaveChanges();
            }
            return RedirectToAction("Hizmetler");
        }
    }
}
