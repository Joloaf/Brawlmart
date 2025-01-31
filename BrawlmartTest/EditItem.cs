using System;
using BrawlmartTest.Models;

namespace BrawlmartTest
{
    internal static class EditItem
    {
        internal static void DisplayAndEditProductDetails(Product product, MyDbContext dbContext, Structure structure, Menu mainMenu, Menu subMenu)
        {
            string[] details = {
                        $"Name: {product.Name}",
                        $"Color: {product.Color}",
                        $"Material: {product.Material}",
                        $"Level: {product.Level}",
                        $"Size: {product.Size}",
                        $"Rarity: {product.Rarity}",
                        $"Details: {product.Details}",
                        $"Stock: {product.Stock}",
                        $"Price: {product.Price}"
                    };

            int selectedIndex = 0;

            while (true)
            {
                Console.Clear();
                Title.DisplayTitle();
                mainMenu.DisplayOptions();
                Console.WriteLine();
                subMenu.DisplayOptions();
                Console.WriteLine();
                Console.WriteLine("Product Details");
                Console.WriteLine("---------------");

                for (int i = 0; i < details.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine(details[i]);
                    Console.ResetColor();
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                ConsoleKey keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    selectedIndex--;
                    if (selectedIndex < 0)
                    {
                        selectedIndex = details.Length - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    selectedIndex++;
                    if (selectedIndex >= details.Length)
                    {
                        selectedIndex = 0;
                    }
                }
                else if (keyPressed == ConsoleKey.Enter)
                {
                    EditDetail(product, selectedIndex, dbContext, structure, mainMenu);
                    details = UpdateDetailsArray(product);
                }
                else if (keyPressed == ConsoleKey.Escape)
                {
                    return;
                }
            }
        }

        private static void EditDetail(Product product, int selectedIndex, MyDbContext dbContext, Structure structure, Menu mainMenu)
        {
            while (true)
            {
                Console.Clear();
                Title.DisplayTitle();
                mainMenu.DisplayOptions();
                Console.WriteLine();
                Console.WriteLine($"Editing {GetDetailName(selectedIndex)}");
                Console.WriteLine("--------------------");

                if (selectedIndex == 5)
                {
                    Console.WriteLine("Select new rarity:");
                    Console.WriteLine("1. Common");
                    Console.WriteLine("2. Magical");
                    Console.WriteLine("3. Rare");
                    Console.WriteLine("4. Legendary");

                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    string newRarity = keyInfo.Key switch
                    {
                        ConsoleKey.D1 or ConsoleKey.NumPad1 => "Common",
                        ConsoleKey.D2 or ConsoleKey.NumPad2 => "Magical",
                        ConsoleKey.D3 or ConsoleKey.NumPad3 => "Rare",
                        ConsoleKey.D4 or ConsoleKey.NumPad4 => "Legendary",
                        _ => null
                    };

                    if (newRarity != null)
                    {
                        product.Rarity = newRarity;
                        dbContext.SaveChanges();
                        Console.WriteLine("Rarity updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection. Please try again.");
                    }
                }
                else
                {
                    Console.Write($"Enter new value (current: {GetCurrentValue(product, selectedIndex)}): ");
                    string input = Console.ReadLine();

                    if (string.IsNullOrEmpty(input))
                    {
                        Console.WriteLine("No changes made.");
                        Console.WriteLine("Press any key to return to the details list...");
                        Console.ReadKey(true);
                        return;
                    }

                    switch (selectedIndex)
                    {
                        case 0:
                            product.Name = input;
                            break;
                        case 1:
                            product.Color = input;
                            break;
                        case 2:
                            product.Material = input;
                            break;
                        case 3:
                            if (int.TryParse(input, out int level))
                            {
                                product.Level = level;
                            }
                            break;
                        case 4:
                            product.Size = input;
                            break;
                        case 6:
                            product.Details = input;
                            break;
                        case 7:
                            if (int.TryParse(input, out int stock) && stock >= 0 && stock < 1000)
                            {
                                product.Stock = stock;
                            }
                            break;
                        case 8:
                            if (float.TryParse(input, out float price))
                            {
                                product.Price = price;
                            }
                            break;
                        default:
                            Console.WriteLine("This detail cannot be edited.");
                            break;
                    }

                    dbContext.SaveChanges();
                    Console.WriteLine("Detail updated successfully.");
                }
                structure.ReloadFrontPageItems();
                structure.weapons.ReloadProducts();
                structure.armors.ReloadProducts();
                structure.trinkets.ReloadProducts();
                Console.WriteLine("Press any key to return to the details list...");
                Console.ReadKey(true);
                return;
            }
        }

        private static string GetDetailName(int index)
        {
            return index switch
            {
                0 => "Name",
                1 => "Color",
                2 => "Material",
                3 => "Level",
                4 => "Size",
                5 => "Rarity",
                6 => "Details",
                7 => "Stock",
                8 => "Price",
                _ => "Unknown"
            };
        }

        private static string GetCurrentValue(Product product, int index)
        {
            return index switch
            {
                0 => product.Name,
                1 => product.Color,
                2 => product.Material,
                3 => product.Level?.ToString(),
                4 => product.Size,
                5 => product.Rarity,
                6 => product.Details,
                7 => product.Stock?.ToString(),
                8 => product.Price?.ToString(),
                _ => "Unknown"
            };
        }

        private static string[] UpdateDetailsArray(Product product)
        {
            return new string[]
            {
                        $"Name: {product.Name}",
                        $"Color: {product.Color}",
                        $"Material: {product.Material}",
                        $"Level: {product.Level}",
                        $"Size: {product.Size}",
                        $"Rarity: {product.Rarity}",
                        $"Details: {product.Details}",
                        $"Stock: {product.Stock}",
                        $"Price: {product.Price}"
            };
        }
    }
}
