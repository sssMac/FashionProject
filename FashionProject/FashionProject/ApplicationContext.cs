using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FashionProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FashionProject
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Content> Content { get; set; }
        public DbSet<FashionСollection> FashionСollection { get; set; }
        public DbSet<Products> Products { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> option)
            : base(option)
        {

        }

    }
}
