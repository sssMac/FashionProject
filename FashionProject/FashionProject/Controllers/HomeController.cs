using FashionProject.Models;
using FashionProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using FashionProject.Models.ViewModels;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace FashionProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private ApplicationContext db;
        private JwtSecurityToken _token;
        public User CurrentUser { get => GetUser(); }

        public HomeController(ApplicationContext context)
        {
            db = context;
        }

        [HttpPost("settings")]
        public IActionResult Post([FromForm] UserViewModel model)
        {
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

            if (model.CurrentPassword != null && model.NewPassword != null)
            {
                if (CurrentUser.Password == Encryption.EncryptString(model.CurrentPassword))
                {
                    CurrentUser.Password = Encryption.EncryptString(model.NewPassword);
                }
            }

            db.SaveChanges();

            return Ok();
        }


        [HttpGet("currentUser")]
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

        [HttpPost("AddNews")]
        public async Task<IActionResult> PostNews([FromForm] ContentViewModel model)
        {
            if (ModelState.IsValid)
            {
                Content content = await db.Content.FirstOrDefaultAsync(u => u.Title == model.Title);
                byte[] img = null;
                if (model.ContentImg != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(model.ContentImg.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)model.ContentImg.Length);
                    }
                    img = imageData;
                }

                if (content == null)
                {
                    var newContent = new Content
                    {
                        Id = Guid.NewGuid(),
                        Title = model.Title,
                        Category = model.Category,
                        SubTitle = model.SubTitle,
                        Description = model.Description,
                        ContentImg = img,
                        Date = DateTime.Now.Second,
                    };
                    db.Content.Add(newContent);
                    await db.SaveChangesAsync();
                }
                else
                    return BadRequest();
            }
            return Ok();
        }
    }
}