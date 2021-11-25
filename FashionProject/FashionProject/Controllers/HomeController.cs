using FashionProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace FashionProject.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;

        public HomeController(ApplicationContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var currentUser = HttpContext.User;

            if (Request.Cookies["token"] == null || Request.Cookies["token"] == "")
            {
                return View(null);
            }

            var stream = Request.Cookies["token"];
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var id = tokenS.Claims.First(claim => claim.Type == "nameid").Value;

            var user = db.Users.FirstOrDefault(u => u.Id.ToString() == id);

            return View(user);
        }

        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }
    }
}