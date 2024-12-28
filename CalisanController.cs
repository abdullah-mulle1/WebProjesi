using Berber.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Berber.Controllers
{
    [Authorize(Roles = "Calisan")]
    public class CalisanController : Controller
    {
        private readonly BerberContext _context;

        public CalisanController(BerberContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }


        // GET: Display Randevular assigned to the logged-in Çalışan
        // GET: Display Randevular assigned to the logged-in Çalışan
        public IActionResult Randevular()
        {
            // Retrieve the logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Giriş yapılmadı. Lütfen tekrar giriş yapın.");
            }

            // Find the Çalışan by UserID
            var calisan = _context.Calisanlar.FirstOrDefault(c => c.UserID == userId);

            if (calisan == null)
            {
                return NotFound("Çalışan bulunamadı. Bilgilerinizi kontrol edin.");
            }

            // Retrieve all Randevular assigned to this Çalışan
            var randevular = _context.Randevular
                .Include(r => r.hizmet) // Include related Hizmet data
                .Where(r => r.CalisanId == calisan.CalisanId)
                .ToList();

            return View(randevular); // Pass the Randevular to the view
        }
        [HttpPost("Calisan/KabulEt/{id}")]
    
        public IActionResult KabulEt(int id)
        {
            var randevu = _context.Randevular.Find(id);
            if (randevu == null)
            {
                return NotFound();
            }

            randevu.Onay = "Onaylandı";
            _context.SaveChanges();

            return Ok(); // تأكيد نجاح العملية
        }

        [HttpPost("Calisan/Reddet/{id}")]

        public IActionResult Reddet(int id)
        {
            var randevu = _context.Randevular.Find(id);
            if (randevu == null)
            {
                return NotFound();
            }

            randevu.Onay = "Reddedildi";
            _context.SaveChanges();

            return Ok(); // تأكيد نجاح العملية
        }


        

    }
}


