using Microsoft.AspNetCore.Mvc;

namespace FashionProject.Controllers
{
    public class RegistrationLoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        
        public IActionResult Registration()
        {
            return View();
        }
        
    }
}