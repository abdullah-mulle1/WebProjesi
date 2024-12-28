using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Berber.Models;
using System.Threading.Tasks;

namespace Berber.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly BerberContext _context;
        private readonly UserManager<Kullanici> _userManager;

        public AdminController(BerberContext context, UserManager<Kullanici> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: Add a new Calisan
        public IActionResult CalisanEkle()
        {
            ViewBag.Hizmetler = _context.Hizmetler.ToList(); // Pass Hizmetler to the view
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalisanEkle(Calisan calisan, string email, string password, int[] SelectedHizmetler)
        {
            if (ModelState.IsValid)
            {
                // Ensure the email ends with @barber.com
                if (!email.EndsWith("@barber.com"))
                {
                    email = email.Split('@')[0] + "@barber.com";
                }

                // Create a new Identity user
                var user = new Kullanici
                {
                    UserName = email,
                    Email = email,
                    Ad = calisan.Ad
                };

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Calisan");

                    // Link the user to the Calisan entity
                    calisan.UserID = user.Id;

                    // Add selected Hizmetler to the Calisan
                    foreach (var hizmetId in SelectedHizmetler)
                    {
                        calisan.CalisanHizmetler.Add(new CalisanHizmet
                        {
                            HizmetId = hizmetId
                        });
                    }

                    _context.Calisanlar.Add(calisan);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Calisanlar));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewBag.Hizmetler = _context.Hizmetler.ToList(); // Reload Hizmetler in case of errors
            return View(calisan);
        }




        // List all Calisanlar
        // AdminController
        public IActionResult Calisanlar()
        {
            var calisanlar = _context.Calisanlar
                .Include(c => c.CalisanHizmetler)
                .ThenInclude(ch => ch.hizmet) // Include Hizmet data
                .ToList();

            return View(calisanlar);
        }



        // GET: Update an existing Calisan
        public IActionResult CalisanGuncelle(int id)
        {
            var calisan = _context.Calisanlar
                .Include(c => c.CalisanHizmetler)
                .ThenInclude(ch => ch.hizmet)
                .FirstOrDefault(c => c.CalisanId == id);

            if (calisan == null)
            {
                return NotFound();
            }

            ViewBag.Hizmetler = _context.Hizmetler.ToList();
            return View(calisan);
        }

        // POST: Update an existing Calisan
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CalisanGuncelle(Calisan calisan, int[] SelectedHizmetler)
        {
            if (ModelState.IsValid)
            {
                var existingCalisan = _context.Calisanlar
                    .Include(c => c.CalisanHizmetler)
                    .FirstOrDefault(c => c.CalisanId == calisan.CalisanId);

                if (existingCalisan == null)
                {
                    return NotFound();
                }

                // Update Calisan details
                existingCalisan.Ad = calisan.Ad;
                existingCalisan.SaatBaslangic = calisan.SaatBaslangic;
                existingCalisan.SaatBitis = calisan.SaatBitis;

                // Update Hizmetler
                existingCalisan.CalisanHizmetler.Clear();
                foreach (var hizmetId in SelectedHizmetler)
                {
                    existingCalisan.CalisanHizmetler.Add(new CalisanHizmet
                    {
                        CalisanId = calisan.CalisanId,
                        HizmetId = hizmetId
                    });
                }

                _context.SaveChanges();
                return RedirectToAction(nameof(Calisanlar));
            }

            ViewBag.Hizmetler = _context.Hizmetler.ToList();
            return View(calisan);
        }

        // DELETE: Delete a Calisan
        public IActionResult CalisanSil(int id)
        {
            var calisan = _context.Calisanlar.Find(id);
            if (calisan != null)
            {
                _context.Calisanlar.Remove(calisan);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Calisanlar));
        }

        // GET: List all Hizmetler
        public IActionResult Hizmetler()
        {
            var hizmetler = _context.Hizmetler.ToList();
            return View(hizmetler);
        }

        // GET: Add a new Hizmet
        public IActionResult HizmetEkle()
        {
            return View();
        }

        // POST: Add a new Hizmet
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HizmetEkle(Hizmet hizmet)
        {
            if (ModelState.IsValid)
            {
                _context.Hizmetler.Add(hizmet);
                _context.SaveChanges();
                return RedirectToAction(nameof(Hizmetler));
            }

            return View(hizmet);
        }

        // GET: Update an existing Hizmet
        public IActionResult HizmetGuncelle(int id)
        {
            var hizmet = _context.Hizmetler.Find(id);
            if (hizmet == null)
            {
                return NotFound();
            }
            return View(hizmet);
        }

        // POST: Update an existing Hizmet
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HizmetGuncelle(Hizmet hizmet)
        {
            if (ModelState.IsValid)
            {
                _context.Hizmetler.Update(hizmet);
                _context.SaveChanges();
                return RedirectToAction(nameof(Hizmetler));
            }

            return View(hizmet);
        }

        // DELETE: Delete a Hizmet
        public IActionResult HizmetSil(int id)
        {
            var hizmet = _context.Hizmetler.Find(id);
            if (hizmet != null)
            {
                _context.Hizmetler.Remove(hizmet);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Hizmetler));
        }
    }
}
