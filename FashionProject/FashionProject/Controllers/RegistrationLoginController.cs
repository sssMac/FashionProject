using FashionProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using FashionProject.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using FashionProject.Services;

namespace FashionProject.Controllers
{
    public class RegistrationLoginController : Controller
    {
        ApplicationContext db;

        private readonly IJWTAuthManager _jWTAuthManager;

        public RegistrationLoginController(ApplicationContext context, IJWTAuthManager jWTAuthManager)
        {
            db = context;
            _jWTAuthManager = jWTAuthManager;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(UserViewModel model)
        {
            if (ModelState.IsValid && model.ConfirmPassword == model.Password)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    var currentUser = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = model.Email,
                        Username = model.Email,
                        Password = Encryption.EncryptString(model.Password),
                    };
                    db.Users.Add(currentUser);
                    await db.SaveChangesAsync();


                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "User already exists");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (Request.Cookies["token"] != null)
                return RedirectToAction("Login", "RegistrationLogin");

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel model)
        {
            User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == Encryption.EncryptString(model.Password));
            if (user == null)
            {
                return RedirectToAction("Registration", "RegistrationLogin");
            }
            var token = _jWTAuthManager.Authenticate(user);

            if (token == null)
                return RedirectToAction("Login", "RegistrationLogin");
            else
            {
                Response.Cookies.Append("token", token);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            var token = Request.Cookies["token"];
            if (token == null)
                return RedirectToAction("Index", "Home");
            Response.Cookies.Delete("token");
            return RedirectToAction("Index", "Home");
        }


    }
}