using BrawlmartTest.Models;
using Microsoft.EntityFrameworkCore;

namespace BrawlmartTest
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            // Uncomment the following lines to create items...
            // NOT TO BE USED MORE THAN ONCE DURING DEVELOPMENT!
            // CreateCategories.Run();
            // CreateWeapons.Run();
            // CreateArmors.Run();
            // CreateTrinkets.Run();

            Console.ReadKey();
            Title.DisplayTitle();
            LoadingBar.DisplayLoadingBar();
            
            Structure structure = new Structure();
            structure.PageStructure();
        }
    }
}