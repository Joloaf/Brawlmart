using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlmartTest.Models
{
    public class Shipping
    {
        public int Id { get; set; }
        public string? ShippingOption { get; set; }

        // Navigation properties
        public ICollection<Order> Orders { get; set; }
    }
}
