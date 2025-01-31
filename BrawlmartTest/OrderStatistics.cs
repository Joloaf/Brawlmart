using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrawlmartTest.Models;

namespace BrawlmartTest
{
    internal class OrderStatistics
    {
        private static string[] options = {
                "Total number of orders",
                "Total revenue",
                "Average order value",
                "Number of orders during the last 30 days"
            };

        private static int selectedIndex = 0;

        public static void DisplayOrderStatisticsMenu(Menu mainMenu, Menu subMenu)
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
                Console.WriteLine("Order Statistics Menu");
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
                    case "Total number of orders":
                        var totalOrders = dbContext.Orders.Count();
                        Console.WriteLine($"Total number of orders: {totalOrders}");
                        break;
                    case "Total revenue":
                        var totalRevenue = dbContext.OrderProducts.Sum(op => op.Quantity * op.Price);
                        Console.WriteLine($"Total revenue: ${totalRevenue:F2}");
                        break;
                    case "Average order value":
                        var totalOrderValue = dbContext.Orders.Sum(o => o.TotalPrice);
                        var totalOrderCount = dbContext.Orders.Count();
                        var averageOrderValue = totalOrderCount > 0 ? totalOrderValue / totalOrderCount : 0;
                        Console.WriteLine($"Average order value: ${averageOrderValue:F2}");
                        break;
                    case "Number of orders during the last 30 days":
                        var thirtyDaysAgo = DateTime.Now.AddDays(-30);
                        var ordersLast30Days = dbContext.Orders.Count(o => o.Date >= thirtyDaysAgo);
                        Console.WriteLine($"Number of orders during the last 30 days: {ordersLast30Days}");
                        break;
                }
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to return to the order statistics menu...");
            Console.ReadKey(true);
        }
    }
}
