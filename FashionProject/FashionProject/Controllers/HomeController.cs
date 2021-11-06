using FashionProject.Models;
using Microsoft.AspNetCore.Mvc;


namespace FashionProject.Controllers
{
    public class HomeController : Controller
    {
        User user = new User();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Check()
        {
            return View(user);
        }
    }
}