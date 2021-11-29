using FashionProject.Models;
using FashionProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;

namespace FashionProject.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;
        private JwtSecurityToken _token;
        public User CurrentUser { get => GetUser(); }

        public HomeController(ApplicationContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            
            return View(CurrentUser);
        }

        public IActionResult Profile(UserViewModel model)
        {
            if (CurrentUser == null)
            {
                return RedirectToAction("Login", "RegistrationLogin");
            }

            return View(CurrentUser);
        }

        public IActionResult Settings(UserViewModel model)
        {
            if (CurrentUser == null)
            {
                return RedirectToAction("Login", "RegistrationLogin");
            }

            if (model.Avatar != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(model.Avatar.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)model.Avatar.Length);
                }
                CurrentUser.Avatar = imageData;
            }

            if (model.Username != null)
            {
                CurrentUser.Username = model.Username;
            }

            if (model.CurrentPassword != null && model.NewPassword !=null)
            {
                if(CurrentUser.Password == Encryption.EncryptString(model.CurrentPassword))
                {
                    CurrentUser.Password = Encryption.EncryptString(model.NewPassword);
                }
            }


            db.SaveChanges();

            return View(CurrentUser);
        }


        public User GetUser()
        {
            var currentUser = HttpContext.User;

            if (Request.Cookies["token"] == null || Request.Cookies["token"] == "")
            {
                return null;
            }

            var stream = Request.Cookies["token"];
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            _token = jsonToken as JwtSecurityToken;

            var CurrentId = _token.Claims.First(claim => claim.Type == "nameid").Value;

            var user = db.Users.FirstOrDefault(u => u.Id.ToString() == CurrentId);

            return user;
        }
    }
}