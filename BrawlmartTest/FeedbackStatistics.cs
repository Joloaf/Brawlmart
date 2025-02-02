using Microsoft.Data.SqlClient;
using Dapper;
using BrawlmartTest.Models;

namespace BrawlmartTest
{
    internal class FeedbackStatistics
    {
        static string connString = "Server=tcp:viking.database.windows.net,1433;Initial Catalog=System24DB;Persist Security Info=False;User ID=ragnarok;Password=rune!Axe430;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        private static string[] options = {
                        "Best rated product(s)",
                        "Worst rated product(s)",
                        "Happiest customer",
                        "Least happy customer",
                    };

        private static int selectedIndex = 0;

        public static void DisplayFeedbackStatisticsMenu(Menu mainMenu, Menu subMenu)
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
                Console.WriteLine("Feedback Statistics Menu");
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
                    case "Best rated product(s)":
                        using (var connection = new SqlConnection(connString))
                        {
                            var query = @"
                            SELECT TOP 1 ProductId, AVG(UserRating) AS AverageRating
                            FROM Feedbacks
                            GROUP BY ProductId
                            ORDER BY AverageRating DESC";

                            var bestRatedProducts = connection.Query<ProductRating>(query).ToList();
                            foreach (var product in bestRatedProducts)
                            {
                                Console.WriteLine($"ProductId: {product.ProductId}, AverageRating: {product.AverageRating}");
                            }
                        }
                        break;
                    case "Worst rated product(s)":
                        using (var connection = new SqlConnection(connString))
                        {
                            var query = @"
                            SELECT TOP 1 ProductId, AVG(UserRating) AS AverageRating
                            FROM Feedbacks
                            GROUP BY ProductId
                            ORDER BY AverageRating ASC";

                            var worstRatedProducts = connection.Query<ProductRating>(query).ToList();
                            foreach (var product in worstRatedProducts)
                            {
                                Console.WriteLine($"ProductId: {product.ProductId}, AverageRating: {product.AverageRating}");
                            }
                        }
                        break;
                    case "Happiest customer":
                        using (var connection = new SqlConnection(connString))
                        {
                            var query = @"
                            SELECT TOP 1 u.UserName, AVG(f.UserRating) AS AverageRating
                            FROM Feedbacks f
                            JOIN Users u ON f.UserId = u.Id
                            GROUP BY u.UserName
                            ORDER BY AverageRating DESC";

                            var happiestCustomer = connection.Query<CustomerRating>(query).ToList();
                            foreach (var customer in happiestCustomer)
                            {
                                Console.WriteLine($"UserName: {customer.UserName}, AverageRating: {customer.AverageRating}");
                            }
                        }
                        break;
                    case "Least happy customer":
                        using (var connection = new SqlConnection(connString))
                        {
                            var query = @"
                            SELECT TOP 1 u.UserName, AVG(f.UserRating) AS AverageRating
                            FROM Feedbacks f
                            JOIN Users u ON f.UserId = u.Id
                            GROUP BY u.UserName
                            ORDER BY AverageRating ASC";

                            var leastHappyCustomer = connection.Query<CustomerRating>(query).ToList();
                            foreach (var customer in leastHappyCustomer)
                            {
                                Console.WriteLine($"UserName: {customer.UserName}, AverageRating: {customer.AverageRating}");
                            }
                        }
                        break;
                }
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to return to the feedback statistics menu...");
            Console.ReadKey(true);
        }
    }
    public class ProductRating
    {
        public int ProductId { get; set; }
        public double AverageRating { get; set; }
        public int FeedbackCount { get; set; }
    }

    public class CustomerRating
    {
        public string UserName { get; set; }
        public double AverageRating { get; set; }
    }

    public class CustomerFeedback
    {
        public string UserName { get; set; }
        public double AverageRating { get; set; }
        public int ProductsBought { get; set; }
    }
}