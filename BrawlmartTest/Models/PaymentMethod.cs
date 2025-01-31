using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlmartTest.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string? PaymentOption { get; set; }

        // Navigation properties
        public ICollection<Order> Orders { get; set; }
    }
}
