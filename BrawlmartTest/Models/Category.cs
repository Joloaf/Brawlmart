﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlmartTest.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        // Navigation properties
        public ICollection<Product> Products { get; set; }
    }
}
