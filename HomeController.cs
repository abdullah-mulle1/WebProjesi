using Berber.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Berber.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BerberContext _context;

        public HomeController(ILogger<HomeController> logger, BerberContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // عرض قائمة المواعيد
        public async Task<IActionResult> Randevular()
        {
            var randevular = await _context.Rndvrlr
                .Include(r => r.Clsn)
                .Include(r => r.Hzmtr)
                .ToListAsync();
            return View(randevular);
        }

        // إضافة موعد جديد (GET)
        [HttpGet]
        public IActionResult YeniRandevu()
        {
            ViewBag.Calisanlar = _context.Clsnlr.ToList();
            ViewBag.Hizmetler = _context.Hzmtrlr.ToList();
            return View();
        }

        // إضافة موعد جديد (POST)
        [HttpPost]
        public async Task<IActionResult> YeniRandevu(Randevu randevu)
        {
            if (ModelState.IsValid)
            {
                randevu.Kullaniliyor = false; // تحديد القيمة الافتراضية
                _context.Rndvrlr.Add(randevu);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Randevu başarıyla eklendi!";
                return RedirectToAction(nameof(Randevular));
            }

            ViewBag.Calisanlar = _context.Clsnlr.ToList();
            ViewBag.Hizmetler = _context.Hzmtrlr.ToList();
            TempData["Error"] = "Randevu eklenirken bir hata oluştu.";
            return View(randevu);
        }

        // حذف موعد
        [HttpGet]
        public async Task<IActionResult> SilRandevu(int id)
        {
            var randevu = await _context.Rndvrlr
                .Include(r => r.Clsn)
                .Include(r => r.Hzmtr)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (randevu == null)
                return NotFound();

            return View(randevu);
        }

        [HttpPost, ActionName("SilRandevu")]
        public async Task<IActionResult> SilRandevuOnay(int id)
        {
            var randevu = await _context.Rndvrlr.FindAsync(id);
            if (randevu != null)
            {
                _context.Rndvrlr.Remove(randevu);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Randevu başarıyla silindi!";
            }
            else
            {
                TempData["Error"] = "Randevu silinirken bir hata oluştu.";
            }

            return RedirectToAction(nameof(Randevular));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
