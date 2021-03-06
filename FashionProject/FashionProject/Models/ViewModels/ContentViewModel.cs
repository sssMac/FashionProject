using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FashionProject.Models.ViewModels
{
    public class ContentViewModel
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public IFormFile ContentImg { get; set; }
    }
}
