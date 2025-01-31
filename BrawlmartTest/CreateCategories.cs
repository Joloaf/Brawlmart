using BrawlmartTest.Models;

namespace BrawlmartTest
{
    // This class is only used initially to setup categories in the database... NOT TO BE USED IN THE FINAL PRODUCT
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