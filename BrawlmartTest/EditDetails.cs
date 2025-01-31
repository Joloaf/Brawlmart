using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrawlmartTest.Models;

namespace BrawlmartTest
{
    internal static class EditDetails
    {
        internal static void DisplayAndEditUserDetails(User user, Menu mainMenu, Menu subMenu)
        {
            string[] fields = { "UserName", "FirstName", "LastName", "Email", "PhoneNumber", "StreetAddress", "PostalCode", "City", "Country" };
            int selectedIndex = 0;

            while (true)
            {
                Console.Clear();
                Title.DisplayTitle();
                mainMenu.DisplayOptions();
                Console.WriteLine();
                subMenu.DisplayOptions();
                Console.WriteLine();

                for (int i = 0; i < fields.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                    }

                    string value = fields[i] switch
                    {
                        "UserName" => user.UserName,
                        "FirstName" => user.FirstName,
                        "LastName" => user.LastName,
                        "Email" => user.Email,
                        "PhoneNumber" => user.PhoneNumber,
                        "StreetAddress" => user.StreetAddress,
                        "PostalCode" => user.PostalCode,
                        "City" => user.City,
                        "Country" => user.Country,
                        _ => string.Empty
                    };

                    Console.WriteLine($"{fields[i]}: {value}");
                    Console.ResetColor();
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                ConsoleKey keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    selectedIndex--;
                    if (selectedIndex < 0)
                    {
                        selectedIndex = fields.Length - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    selectedIndex++;
                    if (selectedIndex >= fields.Length)
                    {
                        selectedIndex = 0;
                    }
                }
                else if (keyPressed == ConsoleKey.Enter)
                {
                    if (fields[selectedIndex] != "UserName")
                    {
                        EditField(user, fields[selectedIndex]);
                    }
                }
                else if (keyPressed == ConsoleKey.Escape)
                {
                    return;
                }
            }
        }

        private static void EditField(User user, string fieldName)
        {
            Console.Clear();
            Console.WriteLine($"Edit {fieldName}");
            Console.WriteLine("--------------");

            string currentValue = fieldName switch
            {
                "FirstName" => user.FirstName,
                "LastName" => user.LastName,
                "Email" => user.Email,
                "PhoneNumber" => user.PhoneNumber,
                "StreetAddress" => user.StreetAddress,
                "PostalCode" => user.PostalCode,
                "City" => user.City,
                "Country" => user.Country,
                _ => string.Empty
            };

            Console.WriteLine($"Current Value: {currentValue}");
            Console.Write("Enter new value (or press Escape to cancel): ");

            StringBuilder newValueBuilder = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    return;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && newValueBuilder.Length > 0)
                {
                    newValueBuilder.Length--;
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    newValueBuilder.Append(keyInfo.KeyChar);
                    Console.Write(keyInfo.KeyChar);
                }
            }

            string newValue = newValueBuilder.ToString();

            if (ValidateField(fieldName, newValue))
            {
                switch (fieldName)
                {
                    case "FirstName":
                        user.FirstName = CapitalizeFirstLetter(newValue);
                        break;
                    case "LastName":
                        user.LastName = CapitalizeFirstLetter(newValue);
                        break;
                    case "Email":
                        user.Email = newValue;
                        break;
                    case "PhoneNumber":
                        user.PhoneNumber = "+" + newValue;
                        break;
                    case "StreetAddress":
                        user.StreetAddress = newValue;
                        break;
                    case "PostalCode":
                        user.PostalCode = newValue;
                        break;
                    case "City":
                        user.City = CapitalizeFirstLetter(newValue);
                        break;
                    case "Country":
                        user.Country = CapitalizeFirstLetter(newValue);
                        break;
                }

                using (var context = new MyDbContext())
                {
                    context.Users.Update(user);
                    context.SaveChanges();
                }

                Console.WriteLine($"\n{fieldName} updated successfully.");
            }
            else
            {
                Console.WriteLine($"\nInvalid value for {fieldName}. Please try again.");
            }

            Console.WriteLine("Press any key to return to the details menu...");
            Console.ReadKey(true);
        }

        private static bool ValidateField(string fieldName, string value)
        {
            switch (fieldName)
            {
                case "FirstName":
                case "LastName":
                    return !string.IsNullOrEmpty(value) && value.All(char.IsLetter) && value.Length <= 30;
                case "Email":
                    return !string.IsNullOrEmpty(value) && value.Contains("@");
                case "PhoneNumber":
                    return !string.IsNullOrEmpty(value) && value.All(char.IsDigit) && value.Length >= 8 && value.Length <= 12;
                case "StreetAddress":
                    return !string.IsNullOrEmpty(value) && value.Length <= 50;
                case "PostalCode":
                    return !string.IsNullOrEmpty(value) && value.All(char.IsLetterOrDigit) && value.Length >= 5 && value.Length <= 10;
                case "City":
                case "Country":
                    return !string.IsNullOrEmpty(value) && value.All(char.IsLetter) && value.Length <= 30;
                default:
                    return false;
            }
        }

        private static string CapitalizeFirstLetter(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return char.ToUpper(value[0]) + value.Substring(1).ToLower();
        }
    }
}
