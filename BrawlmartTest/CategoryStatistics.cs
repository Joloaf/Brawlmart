using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrawlmartTest.Models;

namespace BrawlmartTest
{
    internal class CategoryStatistics
    {
        private static readonly string[] options = {
                "Total number of categories",
                "Products per category",
                "Best selling category",
                "Most viewed category"
            };
        private static int selectedIndex = 0;

        internal static void DisplayCategoryStatisticsMenu(Menu mainMenu, Menu subMenu)
        {
            ConsoleKey key;
            do
            {
                Console.Clear();
                Title.DisplayTitle();
                mainMenu.DisplayOptions();
                Console.WriteLine();
                subMenu.DisplayOptions();
                Console.WriteLine();
                Console.WriteLine("Category Statistics Menu");
                Console.WriteLine(new string('-', 100));
                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine(options[i]);
                    Console.ResetColor();
                }

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex == 0) ? options.Length - 1 : selectedIndex - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex == options.Length - 1) ? 0 : selectedIndex + 1;
                        break;
                    case ConsoleKey.Enter:
                        DisplaySelectedStatistic(mainMenu, subMenu);
                        break;
                }
            } while (key != ConsoleKey.Escape);
        }

        private static void DisplaySelectedStatistic(Menu mainMenu, Menu subMenu)
        {
            Console.Clear();
            Title.DisplayTitle();
            mainMenu.DisplayOptions();
            Console.WriteLine();
            subMenu.DisplayOptions();
            Console.WriteLine();
            Console.WriteLine("Product Statistics");
            Console.WriteLine(new string('-', 100));

            switch (options[selectedIndex])
            {
                case "Total number of categories":
                    DisplayTotalNumberOfCategories();
                    break;
                case "Products per category":
                    DisplayProductsPerCategory();
                    break;
                case "Best selling category":
                    DisplayBestSellingCategory();
                    break;
                case "Most viewed category":
                    DisplayMostViewedCategory();
                    break;
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to return to the statistics menu...");
            Console.ReadKey(true);
        }

        private static void DisplayTotalNumberOfCategories()
        {
            using (var dbContext = new MyDbContext())
            {
                int totalCategories = dbContext.Categories.Count();
                Console.WriteLine($"Total number of categories: {totalCategories}");
            }
        }

        private static void DisplayProductsPerCategory()
        {
            using (var dbContext = new MyDbContext())
            {
                var productsPerCategory = dbContext.Categories
                    .Select(c => new
                    {
                        CategoryName = c.Name,
                        ProductCount = c.Products.Count
                    })
                    .ToList();

                Console.WriteLine("Products per category:");
                foreach (var category in productsPerCategory)
                {
                    Console.WriteLine($"{category.CategoryName}: {category.ProductCount}");
                }
            }
        }

        private static void DisplayBestSellingCategory()
        {
            using (var dbContext = new MyDbContext())
            {
                var productSums = dbContext.Products
                    .Select(p => new
                    {
                        p.CategoryId,
                        TotalQuantity = p.OrderProducts.Sum(op => op.Quantity)
                    })
                    .ToList();

                var bestSellingCategory = productSums
                    .GroupBy(p => p.CategoryId)
                    .Select(g => new
                    {
                        CategoryId = g.Key,
                        TotalSold = g.Sum(p => p.TotalQuantity)
                    })
                    .OrderByDescending(c => c.TotalSold)
                    .FirstOrDefault();

                if (bestSellingCategory != null)
                {
                    var categoryName = dbContext.Categories
                        .Where(c => c.Id == bestSellingCategory.CategoryId)
                        .Select(c => c.Name)
                        .FirstOrDefault();

                    Console.WriteLine($"Best Selling Category: {categoryName} with {bestSellingCategory.TotalSold} items sold.");
                }
            }
        }

        private static void DisplayMostViewedCategory()
        {
            using (var dbContext = new MyDbContext())
            {
                var mostViewedCategories = dbContext.Categories
                    .Select(c => new
                    {
                        CategoryName = c.Name,
                        TotalClicks = c.Products.Sum(p => p.Click ?? 0)
                    })
                    .OrderByDescending(c => c.TotalClicks)
                    .ToList();

                Console.WriteLine("Most viewed categories:");
                foreach (var category in mostViewedCategories)
                {
                    Console.WriteLine($"{category.CategoryName}: {category.TotalClicks} clicks");
                }
            }
        }
    }
}
