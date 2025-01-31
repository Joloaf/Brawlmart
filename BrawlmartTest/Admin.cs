using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrawlmartTest.Models;
using Microsoft.EntityFrameworkCore;


namespace BrawlmartTest
{
    internal static class Admin
    {
        internal static void DisplayAdminPage(User user, Structure structure, Menu mainMenu)
        {
            string[] options = { "Products", "Users", "Orders", "Statistics", "Logout" };
            Menu subMenu = new Menu(options);
            subMenu.SelectedIndex = 0;

            while (true)
            {
                Console.Clear();
                Title.DisplayTitle();
                mainMenu.DisplayOptions();
                Console.WriteLine();
                subMenu.DisplayOptions();

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                ConsoleKey keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.LeftArrow)
                {
                    subMenu.SelectedIndex--;
                    if (subMenu.SelectedIndex < 0)
                    {
                        subMenu.SelectedIndex = options.Length - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.RightArrow)
                {
                    subMenu.SelectedIndex++;
                    if (subMenu.SelectedIndex >= options.Length)
                    {
                        subMenu.SelectedIndex = 0;
                    }
                }
                else if (keyPressed == ConsoleKey.Enter)
                {
                    if (subMenu.SelectedIndex == 0)
                    {
                        DisplayProducts(mainMenu, subMenu, structure);
                    }
                    else if (subMenu.SelectedIndex == 1)
                    {
                        DisplayDetails(mainMenu, subMenu);
                    }
                    else if (subMenu.SelectedIndex == 2)
                    {
                        DisplayOrders(mainMenu, subMenu);
                    }
                    else if (subMenu.SelectedIndex == 3)
                    {
                        Statistics.DisplayStatisticsMenu(mainMenu, subMenu);
                    }
                    else if (subMenu.SelectedIndex == 4)
                    {
                        structure.SetCurrentUser(null);
                        structure.RunMainMenu();
                        return;
                    }
                }
                else if (keyPressed == ConsoleKey.Escape)
                {
                    return;
                }
            }
        }

        private static void DisplayDetails(Menu mainMenu, Menu subMenu)
        {
            using (var dbContext = new MyDbContext())
            {
                var users = dbContext.Users.ToList();
                int selectedIndex = 0;
                int currentPage = 0;
                int pageSize = 25;
                int totalPages = (int)Math.Ceiling(users.Count / (double)pageSize);

                while (true)
                {
                    Console.Clear();
                    Title.DisplayTitle();
                    mainMenu.DisplayOptions();
                    Console.WriteLine();
                    subMenu.DisplayOptions();
                    Console.WriteLine();
                    Console.WriteLine("{0,-20} {1,-20} {2,-20} {3,-30}", "UserName", "FirstName", "LastName", "Email");
                    Console.WriteLine(new string('-', 100));

                    var usersToDisplay = users.Skip(currentPage * pageSize).Take(pageSize).ToList();

                    for (int i = 0; i < usersToDisplay.Count; i++)
                    {
                        if (i == selectedIndex)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;
                        }
                        Console.WriteLine("{0,-20} {1,-20} {2,-20} {3,-37}", usersToDisplay[i].UserName, usersToDisplay[i].FirstName, usersToDisplay[i].LastName, usersToDisplay[i].Email);
                        Console.ResetColor();
                    }

                    Console.WriteLine();
                    Console.WriteLine($"Page {currentPage + 1} of {totalPages}");
                    Console.WriteLine("Use the arrow keys to navigate, Enter to edit, and Escape to return...");

                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    ConsoleKey keyPressed = keyInfo.Key;

                    if (keyPressed == ConsoleKey.UpArrow)
                    {
                        selectedIndex--;
                        if (selectedIndex < 0)
                        {
                            selectedIndex = usersToDisplay.Count - 1;
                        }
                    }
                    else if (keyPressed == ConsoleKey.DownArrow)
                    {
                        selectedIndex++;
                        if (selectedIndex >= usersToDisplay.Count)
                        {
                            selectedIndex = 0;
                        }
                    }
                    else if (keyPressed == ConsoleKey.LeftArrow)
                    {
                        if (currentPage > 0)
                        {
                            currentPage--;
                            selectedIndex = 0;
                        }
                    }
                    else if (keyPressed == ConsoleKey.RightArrow)
                    {
                        if (currentPage < totalPages - 1)
                        {
                            currentPage++;
                            selectedIndex = 0;
                        }
                    }
                    else if (keyPressed == ConsoleKey.Enter)
                    {
                        EditDetails.DisplayAndEditUserDetails(usersToDisplay[selectedIndex], mainMenu, subMenu);
                    }
                    else if (keyPressed == ConsoleKey.Escape)
                    {
                        return;
                    }
                }
            }
        }

        internal static void DisplayProducts(Menu mainMenu, Menu subMenu, Structure structure)
        {
            using (var dbContext = new MyDbContext())
            {
                var products = dbContext.Products.ToList();
                var categories = dbContext.Categories.ToDictionary(c => c.Id, c => c.Name);
                int selectedIndex = 0;
                int currentPage = 0;
                int pageSize = 25;
                int totalPages = (int)Math.Ceiling(products.Count / (double)pageSize);

                while (true)
                {
                    Console.Clear();
                    Title.DisplayTitle();
                    mainMenu.DisplayOptions();
                    Console.WriteLine();
                    subMenu.DisplayOptions();
                    Console.WriteLine();
                    Console.WriteLine("{0,-38} {1,-12} {2,-11} {3,-12} {4,0} {5,9}", "Name", "| [C]ategory", "| [F]rontAd", "|   [R]arity", "| [Q]uantity", "|  [P]rice");
                    Console.WriteLine(new string('-', 100));

                    var productsToDisplay = products.Skip(currentPage * pageSize).Take(pageSize).ToList();

                    for (int i = 0; i < productsToDisplay.Count; i++)
                    {
                        if (i == selectedIndex)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;
                        }
                        string category = categories.ContainsKey(productsToDisplay[i].CategoryId) ? categories[productsToDisplay[i].CategoryId] : "Unknown";
                        Console.WriteLine($"{productsToDisplay[i].Name,-38} | {category,10} | {productsToDisplay[i].FrontId,9} | {productsToDisplay[i].Rarity,10} | {productsToDisplay[i].Stock,10} | {productsToDisplay[i].Price,8}");
                        Console.ResetColor();
                    }

                    Console.WriteLine();
                    Console.WriteLine($"Page {currentPage + 1} of {totalPages}");
                    Console.WriteLine("Use the arrow keys to navigate, The title [K]eys to quick edit, Enter to edit details...");
                    Console.WriteLine("A to add a new item, X to remove an item, and Escape to return...");

                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    ConsoleKey keyPressed = keyInfo.Key;

                    if (keyPressed == ConsoleKey.UpArrow)
                    {
                        selectedIndex--;
                        if (selectedIndex < 0)
                        {
                            selectedIndex = productsToDisplay.Count - 1;
                        }
                    }
                    else if (keyPressed == ConsoleKey.DownArrow)
                    {
                        selectedIndex++;
                        if (selectedIndex >= productsToDisplay.Count)
                        {
                            selectedIndex = 0;
                        }
                    }
                    else if (keyPressed == ConsoleKey.LeftArrow)
                    {
                        if (currentPage > 0)
                        {
                            currentPage--;
                            selectedIndex = 0;
                        }
                    }
                    else if (keyPressed == ConsoleKey.RightArrow)
                    {
                        if (currentPage < totalPages - 1)
                        {
                            currentPage++;
                            selectedIndex = 0;
                        }
                    }
                    else if (keyPressed == ConsoleKey.Enter)
                    {
                        EditItem.DisplayAndEditProductDetails(productsToDisplay[selectedIndex], dbContext, structure, mainMenu, subMenu);
                    }
                    else if (keyPressed == ConsoleKey.A)
                    {
                        CreateItem.AddNewItem(mainMenu, subMenu, structure);
                        // Reload products after adding a new item
                        products = dbContext.Products.ToList();
                        totalPages = (int)Math.Ceiling(products.Count / (double)pageSize);
                    }
                    else if (keyPressed == ConsoleKey.P)
                    {
                        EditProductPrice(mainMenu, subMenu, productsToDisplay[selectedIndex], dbContext);
                        structure.ReloadFrontPageItems();
                        structure.weapons.ReloadProducts();
                        structure.armors.ReloadProducts();
                        structure.trinkets.ReloadProducts();
                        break;
                    }
                    else if (keyPressed == ConsoleKey.R)
                    {
                        EditProductRarity(mainMenu, subMenu, productsToDisplay[selectedIndex], dbContext);
                        structure.ReloadFrontPageItems();
                        structure.weapons.ReloadProducts();
                        structure.armors.ReloadProducts();
                        structure.trinkets.ReloadProducts();
                        break;
                    }
                    else if (keyPressed == ConsoleKey.Q)
                    {
                        EditProductStock(mainMenu, subMenu, productsToDisplay[selectedIndex], dbContext);
                        structure.ReloadFrontPageItems();
                        structure.weapons.ReloadProducts();
                        structure.armors.ReloadProducts();
                        structure.trinkets.ReloadProducts();
                        break;
                    }
                    else if (keyPressed == ConsoleKey.F)
                    {
                        EditProductFrontId(mainMenu, subMenu, productsToDisplay[selectedIndex], dbContext);
                        structure.ReloadFrontPageItems();
                        structure.weapons.ReloadProducts();
                        structure.armors.ReloadProducts();
                        structure.trinkets.ReloadProducts();
                        break;
                    }
                    else if (keyPressed == ConsoleKey.C)
                    {
                        EditProductCategory(mainMenu, subMenu, productsToDisplay[selectedIndex], dbContext);
                        structure.ReloadFrontPageItems();
                        structure.weapons.ReloadProducts();
                        structure.armors.ReloadProducts();
                        structure.trinkets.ReloadProducts();
                        break;
                    }
                    else if (keyPressed == ConsoleKey.X)
                    {
                        RemoveProduct(mainMenu,subMenu, productsToDisplay[selectedIndex], dbContext, structure);
                        // Reload products after removing an item
                        products = dbContext.Products.ToList();
                        totalPages = (int)Math.Ceiling(products.Count / (double)pageSize);
                    }
                    else if (keyPressed == ConsoleKey.Escape)
                    {
                        return;
                    }
                }
            }
        }

        private static void RemoveProduct(Menu mainMenu, Menu subMenu, Product product, MyDbContext dbContext, Structure structure)
        {
            Console.Clear();
            Title.DisplayTitle();
            mainMenu.DisplayOptions();
            Console.WriteLine();
            subMenu.DisplayOptions();
            Console.WriteLine();
            Console.WriteLine("Remove Product");
            Console.WriteLine("--------------");
            Console.WriteLine($"Are you sure you want to remove the product '{product.Name}'? (Y/N)");

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Y)
            {
                dbContext.Products.Remove(product);
                dbContext.SaveChanges();
                Console.WriteLine();
                Console.WriteLine("Product removed successfully.");
                structure.ReloadFrontPageItems();
                structure.weapons.ReloadProducts();
                structure.armors.ReloadProducts();
                structure.trinkets.ReloadProducts();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Product removal canceled.");
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to return to the product list...");
            Console.ReadKey(true);
        }

        internal static void DisplayOrders(Menu mainMenu, Menu subMenu)
        {
            using (var dbContext = new MyDbContext())
            {
                var orders = dbContext.Orders.Include(o => o.User).ToList();
                int selectedIndex = 0;
                int currentPage = 0;
                int pageSize = 25;
                int totalPages = (int)Math.Ceiling(orders.Count / (double)pageSize);

                while (true)
                {
                    Console.Clear();
                    Title.DisplayTitle();
                    mainMenu.DisplayOptions();
                    Console.WriteLine();
                    subMenu.DisplayOptions();
                    Console.WriteLine();
                    Console.WriteLine("{0,-8} {1,0} {2,17} {3,27} {4,14} {5,16}", "  Order ID |", "User Name", "| Name", "|", "Date |", "Total Price");
                    Console.WriteLine(new string('-', 100));

                    var ordersToDisplay = orders.Skip(currentPage * pageSize).Take(pageSize).ToList();

                    for (int i = 0; i < ordersToDisplay.Count; i++)
                    {
                        if (i == selectedIndex)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;
                        }

                        var userName = ordersToDisplay[i].User?.UserName ?? "Unknown";
                        var fullName = $"{ordersToDisplay[i].User?.FirstName ?? "Unknown"} {ordersToDisplay[i].User?.LastName ?? "Unknown"}";

                        Console.WriteLine($"{ordersToDisplay[i].Id,10} | {userName,-20} | {fullName,-30} |   {ordersToDisplay[i].Date:dd/MM/yyyy} | {ordersToDisplay[i].TotalPrice,11} gold");
                        Console.ResetColor();
                    }

                    Console.WriteLine();
                    Console.WriteLine($"Page {currentPage + 1} of {totalPages}");
                    Console.WriteLine("Use the arrow keys to navigate, Enter to view details, and Escape to return...");

                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    ConsoleKey keyPressed = keyInfo.Key;

                    if (keyPressed == ConsoleKey.UpArrow)
                    {
                        selectedIndex--;
                        if (selectedIndex < 0)
                        {
                            selectedIndex = ordersToDisplay.Count - 1;
                        }
                    }
                    else if (keyPressed == ConsoleKey.DownArrow)
                    {
                        selectedIndex++;
                        if (selectedIndex >= ordersToDisplay.Count)
                        {
                            selectedIndex = 0;
                        }
                    }
                    else if (keyPressed == ConsoleKey.LeftArrow)
                    {
                        if (currentPage > 0)
                        {
                            currentPage--;
                            selectedIndex = 0;
                        }
                    }
                    else if (keyPressed == ConsoleKey.RightArrow)
                    {
                        if (currentPage < totalPages - 1)
                        {
                            currentPage++;
                            selectedIndex = 0;
                        }
                    }
                    else if (keyPressed == ConsoleKey.Enter)
                    {
                        OrderHistory.DisplayOrderDetails(ordersToDisplay[selectedIndex], mainMenu, subMenu);
                    }
                    else if (keyPressed == ConsoleKey.Escape)
                    {
                        return;
                    }
                }
            }
        }

        private static void EditProductPrice(Menu mainMenu, Menu subMenu, Product product, MyDbContext dbContext)
        {
            Console.Clear();
            Title.DisplayTitle();
            mainMenu.DisplayOptions();
            Console.WriteLine();
            subMenu.DisplayOptions();
            Console.WriteLine();
            Console.WriteLine("Edit Product Price");
            Console.WriteLine("------------------");
            Console.WriteLine($"Current Price: {product.Price}");
            Console.Write("Enter new price: ");
            string input = Console.ReadLine();

            if (float.TryParse(input, out float newPrice))
            {
                product.Price = newPrice;
                dbContext.SaveChanges();
                Console.WriteLine("Price updated successfully.");
            }
            else
            {
                Console.WriteLine("Invalid price. Please try again.");
            }

            Console.WriteLine("Press any key to return to the product list...");
            Console.ReadKey(true);
        }

        private static void EditProductRarity(Menu mainMenu, Menu subMenu, Product product, MyDbContext dbContext)
        {
            Console.Clear();
            Title.DisplayTitle();
            mainMenu.DisplayOptions();
            Console.WriteLine();
            subMenu.DisplayOptions();
            Console.WriteLine();
            Console.WriteLine("Edit Product Rarity");
            Console.WriteLine("-------------------");
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

            Console.WriteLine("Press any key to return to the product list...");
            Console.ReadKey(true);
        }

        private static void EditProductStock(Menu mainMenu, Menu subMenu, Product product, MyDbContext dbContext)
        {
            Console.Clear();
            Title.DisplayTitle();
            mainMenu.DisplayOptions();
            Console.WriteLine();
            subMenu.DisplayOptions();
            Console.WriteLine();
            Console.WriteLine("Edit Product Stock");
            Console.WriteLine("------------------");
            Console.WriteLine($"Current Stock: {product.Stock}");
            Console.Write("Enter new stock quantity (0-999): ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int newStock) && newStock >= 0 && newStock < 1000)
            {
                product.Stock = newStock;
                dbContext.SaveChanges();
                Console.WriteLine("Stock updated successfully.");
            }
            else
            {
                Console.WriteLine("Invalid stock quantity. Please try again.");
            }

            Console.WriteLine("Press any key to return to the product list...");
            Console.ReadKey(true);
        }

        private static void EditProductFrontId(Menu mainMenu, Menu subMenu, Product product, MyDbContext dbContext)
        {
            Console.Clear();
            Title.DisplayTitle();
            mainMenu.DisplayOptions();
            Console.WriteLine();
            subMenu.DisplayOptions();
            Console.WriteLine();
            Console.WriteLine("Edit Product FrontId");
            Console.WriteLine("--------------------");
            Console.WriteLine($"Current FrontId: {product.FrontId}");
            Console.Write("Enter new FrontId (1-3): ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int newFrontId) && newFrontId >= 1 && newFrontId <= 3)
            {
                // Set the current FrontId of the same value to null
                var existingProduct = dbContext.Products.FirstOrDefault(p => p.FrontId == newFrontId);
                if (existingProduct != null)
                {
                    existingProduct.FrontId = null;
                }

                product.FrontId = newFrontId;
                dbContext.SaveChanges();
                Console.WriteLine("FrontId updated successfully.");
            }
            else
            {
                Console.WriteLine("Invalid FrontId. Please try again.");
            }

            Console.WriteLine("Press any key to return to the product list...");
            Console.ReadKey(true);
        }
        private static void EditProductCategory(Menu mainMenu, Menu subMenu, Product product, MyDbContext dbContext)
        {
            Console.Clear();
            Title.DisplayTitle();
            mainMenu.DisplayOptions();
            Console.WriteLine();
            subMenu.DisplayOptions();
            Console.WriteLine();
            Console.WriteLine("Edit Product Category");
            Console.WriteLine("---------------------");

            var categories = dbContext.Categories.ToList();
            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {categories[i].Name}");
            }

            Console.Write("Select new category: ");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            int selectedIndex = keyInfo.KeyChar - '1';

            if (selectedIndex >= 0 && selectedIndex < categories.Count)
            {
                product.CategoryId = categories[selectedIndex].Id;
                dbContext.SaveChanges();
                Console.WriteLine("Category updated successfully.");
            }
            else
            {
                Console.WriteLine("Invalid selection. Please try again.");
            }

            Console.WriteLine("Press any key to return to the product list...");
            Console.ReadKey(true);
        }
    }
}
