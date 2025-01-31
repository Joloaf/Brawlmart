using BrawlmartTest.Models;
using Microsoft.EntityFrameworkCore;

namespace BrawlmartTest
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            // Uncomment the following lines to create items... NOT TO BE USED IN THE FINAL PRODUCT
            // CreateCategories.Run();
            // CreateWeapons.Run();
            // CreateArmors.Run();
            // CreateTrinkets.Run();

            Title.DisplayTitle();
            LoadingBar.DisplayLoadingBar();
            
            await LoadDatabasesAsync();
            
            Structure structure = new Structure();
            structure.PageStructure();
        }

        private static async Task LoadDatabasesAsync()
        {
            using (var dbContext = new MyDbContext())
            {
                await dbContext.Users.LoadAsync();
                await dbContext.Categories.LoadAsync();
                await dbContext.Products.LoadAsync();
                await dbContext.Orders.LoadAsync();
                await dbContext.Feedbacks.LoadAsync();
            }
        }
    }
}