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
        public ApplicationContext(DbContextOptions<ApplicationContext> option)
            : base(option)
        {

        }

    }
}
