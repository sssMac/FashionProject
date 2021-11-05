using Microsoft.AspNetCore.Mvc;

namespace FashionProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}