﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BrawlmartTest.Models
{
    public class MyDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Order> Orders { get; set; }
        //public DbSet<Shipping> Shippings { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        //public DbSet<PaymentMethod> Payments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=EFKardinalitetsDemo; Trusted_Connection=True; TrustServerCertificate=True;");
            optionsBuilder.UseSqlServer("Server=tcp:viking.database.windows.net,1433;Initial Catalog=System24DB;Persist Security Info=False;User ID=ragnarok;Password=rune!Axe430;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }
}