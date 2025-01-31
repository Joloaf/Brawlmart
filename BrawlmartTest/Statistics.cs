using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrawlmartTest.Models;

namespace BrawlmartTest
{
    internal class Statistics
    {
        private static readonly string[] options = { "Users", "Feedback", "Categories", "Products", "Orders" };
        private static int selectedIndex = 0;

        internal static void DisplayStatisticsMenu(Menu mainMenu, Menu subMenu)
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
                Console.WriteLine("Statistics Menu");
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
            switch (options[selectedIndex])
            {
                case "Users":
                    UserStatistics.DisplayUserStatisticsMenu(mainMenu, subMenu);
                    break;
                case "Feedback":
                    FeedbackStatistics.DisplayFeedbackStatisticsMenu(mainMenu, subMenu);
                    break;
                case "Categories":
                    CategoryStatistics.DisplayCategoryStatisticsMenu(mainMenu, subMenu);
                    break;
                case "Products":
                    ProductStatistics.DisplayProductStatisticsMenu(mainMenu, subMenu);
                    break;
                case "Orders":
                    OrderStatistics.DisplayOrderStatisticsMenu(mainMenu, subMenu);
                    break;
            }
            //Console.WriteLine("Press any key to return to the statistics menu...");
            //Console.ReadKey(true);
        }
    }
}
