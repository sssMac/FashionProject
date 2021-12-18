using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FashionProject.Models.ViewModels
{
    public class CommentViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ContentId { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public long Date { get; set; }
    }
}
