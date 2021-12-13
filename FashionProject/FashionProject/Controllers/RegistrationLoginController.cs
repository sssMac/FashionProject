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
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationLoginController : ControllerBase
    {
        ApplicationContext db;

        private readonly IJWTAuthManager _jWTAuthManager;

        public RegistrationLoginController(ApplicationContext context, IJWTAuthManager jWTAuthManager)
        {
            db = context;
            _jWTAuthManager = jWTAuthManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> PostReg([FromForm] UserViewModel model)
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
                }
                else
                    ModelState.AddModelError("", "User already exists");
            }
            return Ok();

        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> PostLogin([FromForm] UserViewModel model)
        {
            User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == Encryption.EncryptString(model.Password));
            if (user != null)
            {
                var token = _jWTAuthManager.Authenticate(user);

                if (token != null)
                {
                    Response.Cookies.Append("token", token);
                    return Ok();
                }
            }
            return BadRequest();
        }

        [Authorize]
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            var token = Request.Cookies["token"];
            if (token != null)
                Response.Cookies.Delete("token");
            return Ok();
        }


    }
}