using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FashionProject.Models
{
    public class ContentComment
    {
        public Guid Id { get; set; }
        public Guid CommentId{ get; set; }
        public Guid ContentId { get; set; }

    }
}
