using BrawlmartTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlmartTest
{
    internal class CreateCategories
    {
        public static void Run()
        {
            using var context = new Models.MyDbContext();
            var category1 = new Category { Name = "Weapons" };
            context.Add<Category>(category1);
            var category2 = new Category { Name = "Armors" };
            context.Add<Category>(category2);
            var category3 = new Category { Name = "Trinkets" };
            context.Add<Category>(category3);
            context.SaveChanges();
        }
    }
}
