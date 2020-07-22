using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class ApplicationDbContext : DbContext   
    {
        public DbSet<Files> Files { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Data Source=.\sqlexpress;Initial Catalog=DejeroDB;Integrated Security=True");
        }
    }
}
