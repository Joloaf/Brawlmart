using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrawlmartTest.Models;

namespace BrawlmartTest
{
    internal class ProductStatistics
    {
        private static string[] options = {
                "Total number of products",
                "Best selling product(s)",
                "Top 10 products with the most views",
                "Total number of products sold",
                "Average number of products per order",
                "Top 10 products with highest revenue",
                "Number of products in stock",
                "Number of products out of stock"
            };

        private static int selectedIndex = 0;

        public static void DisplayProductStatisticsMenu(Menu mainMenu, Menu subMenu)
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
                Console.WriteLine("Product Statistics Menu");
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

            using (var dbContext = new MyDbContext())
            {
                switch (options[selectedIndex])
                {
                    case "Total number of products":
                        var totalProducts = dbContext.Products.Count();
                        Console.WriteLine($"Total number of products: {totalProducts}");
                        break;
                    case "Best selling product(s)":
                        var bestSellingProducts = dbContext.Products
                            .Select(p => new
                            {
                                ProductName = p.Name,
                                TotalSold = p.OrderProducts.Sum(op => op.Quantity)
                            })
                            .OrderByDescending(p => p.TotalSold)
                            .FirstOrDefault();
                        if (bestSellingProducts != null)
                        {
                            Console.WriteLine($"Best selling product: {bestSellingProducts.ProductName} with {bestSellingProducts.TotalSold} items sold");
                        }
                        break;
                    case "Top 10 products with the most views":
                        var mostViewedProducts = dbContext.Products
                            .OrderByDescending(p => p.Click)
                            .Take(10)
                            .Select(p => new { p.Name, p.Click })
                            .ToList();
                        Console.WriteLine("Top 10 most viewed products:");
                        foreach (var product in mostViewedProducts)
                        {
                            Console.WriteLine($"{product.Name} with {product.Click} views");
                        }
                        break;
                    case "Total number of products sold":
                        var totalProductsSold = dbContext.OrderProducts.Sum(op => op.Quantity);
                        Console.WriteLine($"Total number of products sold: {totalProductsSold}");
                        break;
                    case "Average number of products per order":
                        var averageProductsPerOrder = dbContext.OrderProducts
                            .AsEnumerable() // Switch to client-side evaluation
                            .GroupBy(op => op.OrderId)
                            .Average(g => g.Sum(op => op.Quantity));
                        Console.WriteLine($"Average number of products per order: {averageProductsPerOrder:F2}");
                        break;
                    case "Top 10 products with highest revenue":
                        var revenuePerProduct = dbContext.Products
                            .Select(p => new
                            {
                                ProductName = p.Name,
                                Revenue = p.OrderProducts.Sum(op => op.Quantity * op.Price)
                            })
                            .OrderByDescending(p => p.Revenue)
                            .Take(10)
                            .ToList();
                        Console.WriteLine("Top 10 products by revenue:");
                        foreach (var product in revenuePerProduct)
                        {
                            Console.WriteLine($"{product.ProductName}: ${product.Revenue:F2}");
                        }
                        break;
                    case "Number of products in stock":
                        var productsInStock = dbContext.Products.Count(p => p.Stock > 0);
                        Console.WriteLine($"Number of products in stock: {productsInStock}");
                        break;
                    case "Number of products out of stock":
                        var productsOutOfStock = dbContext.Products.Count(p => p.Stock == 0);
                        Console.WriteLine($"Number of products out of stock: {productsOutOfStock}");
                        break;
                }
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to return to the product statistics menu...");
            Console.ReadKey(true);
        }
    }
}
