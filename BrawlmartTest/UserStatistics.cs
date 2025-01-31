using BrawlmartTest.Models;

namespace BrawlmartTest
{
    internal class UserStatistics
    {
        private static readonly string[] options = {
                    "Total number of users",
                    "Number of users logged in during the last 30 days",
                    "Number of new accounts created during the last 30 days",
                    "Age demographics",
                    "Gender demographics",
                    "Location demographics",
                    "Upcoming birthdays in the next 30 days"
                };
        private static int selectedIndex = 0;

        internal static void DisplayUserStatisticsMenu(Menu mainMenu, Menu subMenu)
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
                Console.WriteLine("User Statistics Menu");
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
            Console.WriteLine("User Statistics");
            Console.WriteLine(new string('-', 100));

            switch (options[selectedIndex])
            {
                case "Total number of users":
                    DisplayTotalNumberOfUsers();
                    break;
                case "Number of users logged in during the last 30 days":
                    DisplayUsersLoggedInLast30Days();
                    break;
                case "Number of new accounts created during the last 30 days":
                    DisplayNewAccountsLast30Days();
                    break;
                case "Age demographics":
                    DisplayAgeDemographics();
                    break;
                case "Gender demographics":
                    DisplayGenderDemographics();
                    break;
                case "Location demographics":
                    DisplayLocationDemographics();
                    break;
                case "Upcoming birthdays in the next 30 days":
                    DisplayUpcomingBirthdays();
                    break;
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to return to the statistics menu...");
            Console.ReadKey(true);
        }

        private static void DisplayTotalNumberOfUsers()
        {
            using (var dbContext = new MyDbContext())
            {
                int totalUsers = dbContext.Users.Count();
                Console.WriteLine($"Total number of users: {totalUsers}");
            }
        }

        private static void DisplayUsersLoggedInLast30Days()
        {
            using (var dbContext = new MyDbContext())
            {
                var usersLoggedInLast30Days = dbContext.Users
                    .Where(u => u.LastLoginDate.HasValue && u.LastLoginDate.Value >= DateTime.Now.AddDays(-30))
                    .ToList();

                Console.WriteLine($"Number of users logged in during the last 30 days: {usersLoggedInLast30Days.Count}");
                Console.WriteLine();
                Console.WriteLine("{0,-20} {1,-20}", "UserName", "Last Login Date");
                Console.WriteLine(new string('-', 40));

                foreach (var user in usersLoggedInLast30Days)
                {
                    Console.WriteLine("{0,-20} {1,-20}", user.UserName, user.LastLoginDate.Value.ToString("yyyy-MM-dd"));
                }
            }
        }

        private static void DisplayNewAccountsLast30Days()
        {
            using (var dbContext = new MyDbContext())
            {
                int newAccountsLast30Days = dbContext.Users.Count(u => u.AccountCreationDate.HasValue && u.AccountCreationDate.Value >= DateTime.Now.AddDays(-30));
                Console.WriteLine($"Number of new accounts created during the last 30 days: {newAccountsLast30Days}");
            }
        }

        private static void DisplayAgeDemographics()
        {
            using (var dbContext = new MyDbContext())
            {
                var users = dbContext.Users.ToList();
                var ageGroups = new[] { "0-9", "10-19", "20-29", "30-39", "40-49", "50-59", "60+" };
                var ageDemographics = new int[ageGroups.Length];
                foreach (var user in users)
                {
                    if (user.DateOfBirth.HasValue)
                    {
                        int age = DateTime.Now.Year - user.DateOfBirth.Value.Year;
                        if (user.DateOfBirth.Value > DateTime.Now.AddYears(-age)) age--;
                        if (age < 10) ageDemographics[0]++;
                        else if (age < 20) ageDemographics[1]++;
                        else if (age < 30) ageDemographics[2]++;
                        else if (age < 40) ageDemographics[3]++;
                        else if (age < 50) ageDemographics[4]++;
                        else if (age < 60) ageDemographics[5]++;
                        else ageDemographics[6]++;
                    }
                }
                Console.WriteLine("Age demographics:");
                for (int i = 0; i < ageGroups.Length; i++)
                {
                    Console.WriteLine($"{ageGroups[i]}: {ageDemographics[i]}");
                }
            }
        }

        private static void DisplayGenderDemographics()
        {
            using (var dbContext = new MyDbContext())
            {
                var users = dbContext.Users.ToList();
                var genderGroups = new[] { "Male", "Female", "Machine", "N/A", "Other" };
                var genderDemographics = new int[genderGroups.Length];
                foreach (var user in users)
                {
                    switch (user.Gender)
                    {
                        case "Male":
                            genderDemographics[0]++;
                            break;
                        case "Female":
                            genderDemographics[1]++;
                            break;
                        case "Machine":
                            genderDemographics[2]++;
                            break;
                        case "Prefer not to disclose":
                            genderDemographics[3]++;
                            break;
                        default:
                            genderDemographics[4]++;
                            break;
                    }
                }
                Console.WriteLine("Gender demographics:");
                for (int i = 0; i < genderGroups.Length; i++)
                {
                    double percentage = (double)genderDemographics[i] / users.Count * 100;
                    Console.WriteLine($"{genderGroups[i]}: {percentage:F2}%");
                }
            }
        }

        private static void DisplayLocationDemographics()
        {
            using (var dbContext = new MyDbContext())
            {
                var users = dbContext.Users.ToList();
                var topCountries = users.GroupBy(u => u.Country)
                                        .Select(g => new { Country = g.Key, Count = g.Count() })
                                        .OrderByDescending(g => g.Count)
                                        .Take(5)
                                        .ToList();

                var topCities = users.GroupBy(u => u.City)
                                     .Select(g => new { City = g.Key, Count = g.Count() })
                                     .OrderByDescending(g => g.Count)
                                     .Take(5)
                                     .ToList();

                Console.WriteLine("Location demographics:");
                Console.WriteLine();
                Console.WriteLine("Top 5 Countries:");
                foreach (var group in topCountries)
                {
                    Console.WriteLine($"{group.Country}: {group.Count}");
                }

                Console.WriteLine();
                Console.WriteLine("Top 5 Cities:");
                foreach (var group in topCities)
                {
                    Console.WriteLine($"{group.City}: {group.Count}");
                }
            }
        }

        private static void DisplayUpcomingBirthdays()
        {
            using (var dbContext = new MyDbContext())
            {
                var users = dbContext.Users.ToList();
                var upcomingBirthdays = users.Where(u => u.DateOfBirth.HasValue && u.DateOfBirth.Value.AddYears(DateTime.Now.Year - u.DateOfBirth.Value.Year) >= DateTime.Now && u.DateOfBirth.Value.AddYears(DateTime.Now.Year - u.DateOfBirth.Value.Year) <= DateTime.Now.AddDays(30)).ToList();
                Console.WriteLine("Upcoming birthdays in the next 30 days:");
                foreach (var user in upcomingBirthdays)
                {
                    Console.WriteLine($"{user.FirstName} {user.LastName} - {user.DateOfBirth.Value.ToString("yyyy-MM-dd")}");
                }
            }
        }
    }
}