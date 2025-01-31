using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlmartTest.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public float TotalPrice { get; set; }
        public int UserId { get; set; }
        //public int ShippingId { get; set; }
        //public int PaymentId { get; set; }

        // Navigation properties
        public User User { get; set; }
        //public Shipping Shipping { get; set; }
        //public PaymentMethod PaymentMethod { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}