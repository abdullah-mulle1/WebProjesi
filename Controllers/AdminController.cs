using Microsoft.AspNetCore.Mvc;
using Berber.Models;
using Microsoft.EntityFrameworkCore;

namespace Berber.Controllers
{

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
        // 1. قائمة الموظفين (Read)
        public async Task<IActionResult> Calisanlar()
        {
            var calisanlar = await _context.Clsnlr.ToListAsync();
            return View(calisanlar);
        }

        // 2. إضافة موظف جديد (Create)
        [HttpGet]
        public IActionResult YeniCalisan()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> YeniCalisan(Calisan calisan)
        {
            if (ModelState.IsValid)
            {
                _context.Clsnlr.Add(calisan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Calisanlar));
            }
            return View(calisan);
        }

        // 3. تعديل موظف (Update)
        [HttpGet]
        public async Task<IActionResult> DuzenleCalisan(int id)
        {
            var calisan = await _context.Clsnlr.FindAsync(id);
            if (calisan == null) return NotFound();
            return View(calisan);
        }

        [HttpPost]
        public async Task<IActionResult> DuzenleCalisan(Calisan calisan)
        {
            if (ModelState.IsValid)
            {
                _context.Clsnlr.Update(calisan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Calisanlar));
            }
            return View(calisan);
        }

        // 4. حذف موظف (Delete)
        [HttpGet]
        public async Task<IActionResult> SilCalisan(int id)
        {
            var calisan = await _context.Clsnlr.FindAsync(id);
            if (calisan == null) return NotFound();
            return View(calisan);
        }

        [HttpPost, ActionName("SilCalisan")]
        public async Task<IActionResult> SilCalisanOnay(int id)
        {
            var calisan = await _context.Clsnlr.FindAsync(id);
            if (calisan != null)
            {
                _context.Clsnlr.Remove(calisan);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Calisanlar));
        }

        // 5. قائمة الخدمات (Read)
        public async Task<IActionResult> Hizmetler()
        {
            var hizmetler = await _context.Hzmtrlr.ToListAsync();
            return View(hizmetler);
        }

        // 6. إضافة خدمة جديدة (Create)
        [HttpGet]
        public IActionResult YeniHizmet()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> YeniHizmet(Hizmet hizmet)
        {
            if (ModelState.IsValid)
            {
                _context.Hzmtrlr.Add(hizmet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Hizmetler));
            }
            return View(hizmet);
        }

        // 7. تعديل خدمة (Update)
        [HttpGet]
        public async Task<IActionResult> DuzenleHizmet(int id)
        {
            var hizmet = await _context.Hzmtrlr.FindAsync(id);
            if (hizmet == null) return NotFound();
            return View(hizmet);
        }

        [HttpPost]
        public async Task<IActionResult> DuzenleHizmet(Hizmet hizmet)
        {
            if (ModelState.IsValid)
            {
                _context.Hzmtrlr.Update(hizmet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Hizmetler));
            }
            return View(hizmet);
        }

        // 8. حذف خدمة (Delete)
        [HttpGet]
        public async Task<IActionResult> SilHizmet(int id)
        {
            var hizmet = await _context.Hzmtrlr.FindAsync(id);
            if (hizmet == null) return NotFound();
            return View(hizmet);
        }

        [HttpPost, ActionName("SilHizmet")]
        public async Task<IActionResult> SilHizmetOnay(int id)
        {
            var hizmet = await _context.Hzmtrlr.FindAsync(id);
            if (hizmet != null)
            {
                _context.Hzmtrlr.Remove(hizmet);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Hizmetler));
        }
    }
}
