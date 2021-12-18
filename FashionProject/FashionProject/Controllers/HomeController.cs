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
using System.Collections.Generic;

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
                        Date = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds(),
                    };
                    db.Content.Add(newContent);
                    await db.SaveChangesAsync();
                }
                else
                    return BadRequest();
            }
            return Ok();
        }

        [HttpGet("GetContent")]
        public List<Content> GetContent()
        {
            List<Content> con =  db.Content.Select(x => x).ToList<Content>();
            return con;
        }

        
        [HttpGet("CreatePage")]
        public Content PostPageId(string id)
        {
            var content = db.Content.FirstOrDefault(u => u.Id.ToString() == id);
            return content;
        }

        [HttpPost("AddComment")]
        public async Task<IActionResult> PostComment([FromForm] CommentViewModel model,string contentId,string userId)
        {
            if (ModelState.IsValid)
            {
                Content comment = await db.Content.FirstOrDefaultAsync(u => u.Id.ToString() == model.Id);
               
                if (comment == null)
                {
                    var newComment = new Comment
                    {
                        Id = Guid.NewGuid(),
                        ContentId = Guid.Parse(contentId),
                        UserId = Guid.Parse(userId),
                        Author = db.Users.FirstOrDefault(u => u.Id.ToString() == userId).Username,
                        Text = model.Text,
                        Date = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds(),
                    };
                    db.Comments.Add(newComment);
                    await db.SaveChangesAsync();
                }
                else
                    return BadRequest();
            }
            return Ok();
        }

        [HttpGet("ViewComment")]
        public List<Comment> GetComment(string contentId)
        {
            List<Comment> comment = db.Comments.Select(x => x).Where(x => x.ContentId.ToString() == contentId).ToList<Comment>();
            return comment;
        }

        [HttpGet("AmountComments")]
        public int GetAmountComment(string contentId)
        {
            List<Comment> comment = db.Comments.Select(c => c).Where(c => c.ContentId.ToString() == contentId).ToList();
            return comment.Count();
        }
    }
}