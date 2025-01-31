using System;
using BrawlmartTest.Models;
using System.Linq;
using System.Text;

namespace BrawlmartTest
{
    internal static class Login
    {
        private static User currentUser;

        public static void SetCurrentUser(User user)
        {
            currentUser = user;
        }

        public static User GetCurrentUser()
        {
            return currentUser;
        }

        public static bool IsLoggedIn()
        {
            return currentUser != null;
        }

        internal static void DisplayLogin(Structure structure, Menu mainMenu)
        {
            Console.Clear();
            Title.DisplayTitle();
            mainMenu.DisplayOptions();

            int leftPosition = 24;
            int topPosition = 12;
            int frameWidth = 48;
            int frameHeight = 8;

            // Top border
            Console.SetCursorPosition(leftPosition, topPosition);
            Console.Write("╔");
            Console.Write(new string('═', frameWidth - 2));
            Console.Write("╗");

            // Side borders
            for (int i = 1; i < frameHeight - 1; i++)
            {
                Console.SetCursorPosition(leftPosition, topPosition + i);
                Console.Write("║");
                Console.SetCursorPosition(leftPosition + frameWidth - 1, topPosition + i);
                Console.Write("║");
            }

            // Bottom border
            Console.SetCursorPosition(leftPosition, topPosition + frameHeight - 1);
            Console.Write("╚");
            Console.Write(new string('═', frameWidth - 2));
            Console.Write("╝");

            // Text for login frame
            Console.SetCursorPosition(leftPosition + 21, topPosition + 1);
            Console.WriteLine("Login");
            Console.SetCursorPosition(leftPosition + 1, topPosition + 2);
            Console.WriteLine("----------------------------------------------");
            Console.SetCursorPosition(leftPosition + 2, topPosition + 3);
            Console.Write("UserName: ");
            string userName = Console.ReadLine();
            Console.SetCursorPosition(leftPosition + 2, topPosition + 4);
            Console.Write("Password: ");
            string password = EncryptPassword();

            using (var dbContext = new MyDbContext())
            {
                var user = dbContext.Users.FirstOrDefault(u => u.UserName == userName && u.Password == password);
                if (user != null)
                {
                    user.LastLoginDate = DateTime.Now;
                    dbContext.SaveChanges();

                    SetCurrentUser(user);
                    structure.SetCurrentUser(user);
                    Console.SetCursorPosition(leftPosition + 2, topPosition + 5);
                    Console.WriteLine("Login successful!");
                    Thread.Sleep(1000);
                    structure.RunMainMenu();
                    return;
                }
                else
                {
                    Console.SetCursorPosition(leftPosition + 2, topPosition + 5);
                    Console.WriteLine("Invalid UserName or Password.");
                }
            }

            Console.SetCursorPosition(leftPosition + 2, topPosition + 6);
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey(true);
        }

        private static string EncryptPassword()
        {
            StringBuilder password = new StringBuilder();
            ConsoleKeyInfo keyInfo;

            while (true)
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password.Remove(password.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    password.Append(keyInfo.KeyChar);
                    Console.Write(keyInfo.KeyChar);
                    System.Threading.Thread.Sleep(200);
                    Console.Write("\b*");
                }
            }

            return password.ToString();
        }
    }
}

