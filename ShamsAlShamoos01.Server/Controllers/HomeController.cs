using Microsoft.AspNetCore.Mvc;

namespace ShamsAlShamoos01.Server.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
