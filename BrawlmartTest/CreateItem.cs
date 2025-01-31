using BrawlmartTest.Models;

namespace BrawlmartTest
{
    internal class CreateItem
    {
        public static void AddNewItem(Menu mainMenu, Menu subMenu, Structure structure)
        {
            Console.Clear();
            Title.DisplayTitle();
            mainMenu.DisplayOptions();
            Console.WriteLine();
            subMenu.DisplayOptions();
            Console.WriteLine();
            Console.WriteLine("Add New Item");
            Console.WriteLine("------------");

            string name = ReadInput("Name: ");
            string color = ReadInput("Color: ");
            string material = ReadInput("Material: ");
            int level = ReadIntInput("Level: ");
            string size = ReadInput("Size: ");
            string rarity = ReadRarityInput();
            string details = ReadInput("Details: ");
            int stock = ReadIntInput("Stock: ");
            float price = ReadFloatInput("Price: ");
            int categoryId = ReadCategoryInput();

            using (var dbContext = new MyDbContext())
            {
                var newItem = new Product
                {
                    Name = name,
                    Color = color,
                    Material = material,
                    Level = level,
                    Size = size,
                    Rarity = rarity,
                    Details = details,
                    Stock = stock,
                    Price = price,
                    CategoryId = categoryId
                };

                dbContext.Products.Add(newItem);
                dbContext.SaveChanges();
                Console.WriteLine("Item added successfully!");
            }

            // Reload products in the marketplace
            structure.ReloadFrontPageItems();
            structure.weapons.ReloadProducts();
            structure.armors.ReloadProducts();
            structure.trinkets.ReloadProducts();

            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey(true);
        }

        private static string ReadInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        private static int ReadIntInput(string prompt)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out value))
                {
                    break;
                }
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
            return value;
        }

        private static float ReadFloatInput(string prompt)
        {
            float value;
            while (true)
            {
                Console.Write(prompt);
                if (float.TryParse(Console.ReadLine(), out value))
                {
                    break;
                }
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
            return value;
        }

        private static int ReadCategoryInput()
        {
            while (true)
            {
                Console.WriteLine("Select Category:");
                Console.WriteLine("1. Weapon");
                Console.WriteLine("2. Armor");
                Console.WriteLine("3. Trinket");
                Console.Write("Enter the number corresponding to the category: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        return 1;
                    case "2":
                        return 2;
                    case "3":
                        return 3;
                    default:
                        Console.WriteLine("Invalid selection. Please try again.");
                        break;
                }
            }
        }

        private static string ReadRarityInput()
        {
            while (true)
            {
                Console.WriteLine("Select Rarity:");
                Console.WriteLine("1. Common");
                Console.WriteLine("2. Magical");
                Console.WriteLine("3. Rare");
                Console.WriteLine("4. Legendary");
                Console.Write("Enter the number corresponding to the rarity: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        return "Common";
                    case "2":
                        return "Magical";
                    case "3":
                        return "Rare";
                    case "4":
                        return "Legendary";
                    default:
                        Console.WriteLine("Invalid selection. Please try again.");
                        break;
                }
            }
        }
    }
}