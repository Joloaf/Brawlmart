using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlmartTest.Models
{
    internal class MongoData
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public int UserInterest { get; set; }
    }
}