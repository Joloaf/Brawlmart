using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlmartTest.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
        public string? Material { get; set; }
        public int? Level { get; set; }
        public string? Size { get; set; }
        public string? Rarity { get; set; }
        public string? Details { get; set; }
        public int? Stock { get; set; }
        public float? Price { get; set; }
        public int CategoryId { get; set; }
        public int? FrontId { get; set; }
        public int? Click { get; set; }

        // Navigation properties
        public Category Category { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
