
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Berber.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Berber.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly BerberContext _context;

        public KullaniciController(BerberContext context)
        {
            _context = context;
        }

        // GET: Kullanici/Index - List all appointments
        public async Task<IActionResult> Index()
        {
            var randevular = await _context.Randevular
                .Include(r => r.calisan)
                .Include(r => r.hizmet)
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

            randevu.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (_context.Randevular.Any(r => r.CalisanId == randevu.CalisanId && r.RndvTarih.Date == randevu.RndvTarih.Date))
            {
                ModelState.AddModelError(string.Empty, "The selected date is already booked for the selected employee.");
                return View(randevu);
            }

          
                _context.Randevular.Add(randevu);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            
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

        // POST: Kullanici/Edit/{id} - Save changes to an appointment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Randevu randevu)
        {
            if (id != randevu.RandevuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(randevu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RandevuExists(randevu.RandevuId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CalisanId"] = new SelectList(_context.Calisanlar, "CalisanId", "Ad", randevu.CalisanId);
            ViewData["HizmetId"] = new SelectList(_context.Hizmetler, "HizmetId", "Ad", randevu.HizmetId);
            return View(randevu);
        }

        // POST: Kullanici/Delete/{id} - Delete an appointment
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var randevu = await _context.Randevular.FindAsync(id);
            if (randevu != null)
            {
                _context.Randevular.Remove(randevu);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Helper: Check if an appointment exists
        private bool RandevuExists(int id)
        {
            return _context.Randevular.Any(e => e.RandevuId == id);
        }
    }
}
