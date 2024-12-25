using Microsoft.AspNetCore.Mvc;

namespace Berber.Controllers
{
    public class CalisanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
