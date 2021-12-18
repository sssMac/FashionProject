using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FashionProject.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ContentId { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public long Date { get; set; }

    }
}