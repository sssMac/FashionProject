using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FashionProject.Models
{
    public class FashionСollection
    {
        public Guid Id { get; set; }
        public string Season { get; set; }
        public string Brand { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public byte[] ContentImg { get; set; }
    }
}
