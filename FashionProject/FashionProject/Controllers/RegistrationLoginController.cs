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

namespace FashionProject.Controllers
{
    public class RegistrationLoginController : Controller
    {
        ApplicationContext db;

        public RegistrationLoginController(ApplicationContext context)
        {
            db = context;
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
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
                        Password = model.Password,

                    };
                    db.Users.Add(currentUser);
                    await db.SaveChangesAsync();


                    return RedirectToAction("Check", "Home");
                }
                else
                    ModelState.AddModelError("", "User already exists");
            }
            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(UserViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

        //        if (user != null)
        //        {
        //            ViewBag.User = user;
        //            return RedirectToAction("Check", "Home");
        //        }
        //        ModelState.AddModelError("", "Некорректные логин и(или) пароль");
        //    }
        //    return View(model);
        //}

        
    }
}