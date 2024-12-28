
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Berber.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Berber.Controllers
{
  //  [Authorize(Roles = "Kullanici")]
    public class KullaniciController : Controller
    {
        private readonly BerberContext _context;

        public KullaniciController(BerberContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var randevular = await _context.Randevular
                .Include(r => r.calisan)
                .Include(r => r.hizmet)
                .Include(r => r.kullanici)
                .ToListAsync();

            return View(randevular);
        }

        public IActionResult Create()
        {
            ViewData["CalisanId"] = new SelectList(_context.Calisanlar, "CalisanId", "Ad");
            ViewData["HizmetId"] = new SelectList(_context.Hizmetler, "HizmetId", "Ad");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Randevu randevu)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("You must be logged in to book an appointment.");
            }

            randevu.UserID = userId; // Assign the logged-in user's ID

            if (!string.IsNullOrEmpty(Request.Form["RndvSaat"]))
            {
                randevu.RndvSaat = TimeSpan.Parse(Request.Form["RndvSaat"]);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please select a time.");
                return View(randevu);
            }

            // Ensure CalisanId is assigned
            if (randevu.CalisanId == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select a Calisan.");
                return View(randevu);
            }

            _context.Randevular.Add(randevu);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult GetHizmetlerByCalisan(int calisanId)
        {
            var hizmetler = _context.Hizmetler
                .Where(h => h.Calisanlar.Any(ch => ch.CalisanId == calisanId))
                .Select(h => new { h.HizmetId, h.Ad })
                .ToList();

            return Json(hizmetler);
        }

        [HttpGet]
        public IActionResult GetAvailableSlots(int calisanId, int hizmetId, DateTime selectedDate)
        {
            var calisan = _context.Calisanlar.FirstOrDefault(c => c.CalisanId == calisanId);
            var hizmet = _context.Hizmetler.FirstOrDefault(h => h.HizmetId == hizmetId);

            if (calisan == null || hizmet == null)
                return Json(new List<string>());

            var startTime = calisan.SaatBaslangic;
            var endTime = calisan.SaatBitis;
            var duration = hizmet.SrDk;

            var availableSlots = new List<string>();
            for (var time = startTime; time + TimeSpan.FromMinutes(duration) <= endTime; time += TimeSpan.FromMinutes(duration))
            {
                var slot = time.ToString(@"hh\:mm") + " - " + (time + TimeSpan.FromMinutes(duration)).ToString(@"hh\:mm");
                var isBooked = _context.Randevular.Any(r => r.CalisanId == calisanId && r.RndvTarih.Date == selectedDate.Date && r.RndvSaat == time);
                if (!isBooked)
                {
                    availableSlots.Add(slot);
                }
            }

            return Json(availableSlots);
        }

        public IActionResult Randevularim()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("You must be logged in to view your appointments.");
            }

            var randevular = _context.Randevular
                .Include(r => r.calisan)
                .Include(r => r.hizmet)
                 .Include(r => r.kullanici)
                .Where(r => r.UserID == userId)
                .ToList();

            return View(randevular);
        }

        // GET: Kullanici/RandevularByCalisan
        public IActionResult RandevularByCalisan(int calisanId)
        {
            var randevular = _context.Randevular
                .Include(r => r.kullanici)
                .Include(r => r.hizmet)
                .Where(r => r.CalisanId == calisanId)
                .ToList();

            return View(randevular);
        }

        // GET: Kullanici/Edit/{id} - Show form to edit an appointment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(int id)
        {
            var randevu = _context.Randevular.Find(id);
            if (randevu != null)
            {
                randevu.Onay = "Onaylandi";
                _context.SaveChanges();
            }
            return RedirectToAction("Randevular");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(int id)
        {
            var randevu = _context.Randevular.Find(id);
            if (randevu != null)
            {
                randevu.Onay = "Reddedildi";
                _context.SaveChanges();
            }
            return RedirectToAction("Randevular");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var randevu = await _context.Randevular.FindAsync(id);
            if (randevu == null)
            {
                return NotFound();
            }

            ViewData["CalisanId"] = new SelectList(_context.Calisanlar, "CalisanId", "Ad", randevu.CalisanId);
            ViewData["HizmetId"] = new SelectList(_context.Hizmetler, "HizmetId", "Ad", randevu.HizmetId);
            return View(randevu);
        }
        private string GetCurrentUserId()
        {
            return User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        }

        // POST: Kullanici/Edit/{id} - Save changes to an appointment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Randevu randevu)
        {
            if (id != randevu.RandevuId)
            {
                return NotFound();
            }

            // تعيين UserID إذا لم يكن موجودًا
            if (string.IsNullOrWhiteSpace(randevu.UserID))
            {
                randevu.UserID = GetCurrentUserId(); // تعيين UserID الحالي
            }

           
                    _context.Update(randevu);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
              

            ViewData["CalisanId"] = new SelectList(_context.Calisanlar, "CalisanId", "Ad", randevu.CalisanId);
            ViewData["HizmetId"] = new SelectList(_context.Hizmetler, "HizmetId", "Ad", randevu.HizmetId);
            return View(randevu);
        }


    }
}
