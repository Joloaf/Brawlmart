using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrawlmartTest.Models;

namespace BrawlmartTest
{
    internal class SearchItem
    {
        public static void Search(Menu mainMenu)
        {
            Console.Clear();
            Title.DisplayTitle();
            mainMenu.DisplayOptions();
            Console.WriteLine();

            Console.Write("Enter search word: ");
            int cursorLeft = Console.CursorLeft;
            int cursorTop = Console.CursorTop;

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press Enter to start the search for your desired item!");
            Console.WriteLine("(Leave the search field empty to rummage through all the potential loot in the marketplace...)");

            // Set the cursor position back to the input line
            Console.SetCursorPosition(cursorLeft, cursorTop);

            // Read the input with the ability to cancel with Escape
            StringBuilder searchTermBuilder = new StringBuilder();
            ConsoleKeyInfo keyInfo;
            while (true)
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    return;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && searchTermBuilder.Length > 0)
                {
                    searchTermBuilder.Length--;
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    searchTermBuilder.Append(keyInfo.KeyChar);
                    Console.Write(keyInfo.KeyChar);
                }
            }

            string searchTerm = searchTermBuilder.ToString();

            List<Product> matchedItems;
            using (var dbContext = new MyDbContext())
            {
                matchedItems = dbContext.Products
                    .Where(p => p.Name.Contains(searchTerm) ||
                                p.Color.Contains(searchTerm) ||
                                p.Rarity.Contains(searchTerm) ||
                                p.Size.Contains(searchTerm) ||
                                p.Material.Contains(searchTerm) ||
                                p.Details.Contains(searchTerm))
                    .OrderBy(p => p.Name) // Sort by Name
                    .ToList();
            }

            if (matchedItems.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("No items matched your search.");
                Console.WriteLine("Press any key to return to the main menu...");
                Console.ReadKey(true);
                return;
            }

            NavigateMatchedItems(matchedItems, mainMenu);
        }

        private static void NavigateMatchedItems(List<Product> matchedItems, Menu mainMenu, int initialIndex = 0)
        {
            int selectedIndex = initialIndex;
            int currentPage = 0;
            int itemsPerPage = 15;
            int totalPages = (int)Math.Ceiling((double)matchedItems.Count / itemsPerPage);
            int itemStartLine = 13; // Adjust this based on the actual line number where items start
            ConsoleKey keyPressed;

            void DisplayPage()
            {
                Console.Clear();
                Title.DisplayTitle();
                mainMenu.DisplayOptions();
                Console.WriteLine();

                Console.WriteLine();
                Console.WriteLine("{0,-35} {1,12} {2,15} {3,15} {4,19}", "Name", "Category", "Level", "Rarity", "Price");
                Console.WriteLine(new string('-', 100));

                int startItemIndex = currentPage * itemsPerPage;
                int endItemIndex = Math.Min(startItemIndex + itemsPerPage, matchedItems.Count);

                using (var dbContext = new MyDbContext())
                {
                    for (int i = startItemIndex; i < endItemIndex; i++)
                    {
                        var item = matchedItems[i];
                        var category = dbContext.Categories.FirstOrDefault(c => c.Id == item.CategoryId)?.Name ?? "Unknown";
                        if (i == selectedIndex)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;
                        }
                        if (item.Stock == 0)
                        {
                            Console.WriteLine("{0,-35} {1,12} {2,15} {3,15} {4,19}", item.Name, category, item.Level, item.Rarity, "   Out of stock");
                        }
                        else
                        {
                            Console.WriteLine("{0,-35} {1,12} {2,15} {3,15} {4,14} gold", item.Name, category, item.Level, item.Rarity, item.Price);
                        }
                        Console.ResetColor();
                    }
                }

                if (totalPages > 1)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Page {currentPage + 1} of {totalPages}");
                    Console.WriteLine("Use Left/Right arrow keys to navigate pages.");
                }
                Console.WriteLine();
                Console.WriteLine("Press B to add an item to your wagon or Enter to view more item details...");
            }

            DisplayPage();

            void UpdateSelection(int oldIndex, int newIndex)
            {
                int startItemIndex = currentPage * itemsPerPage;
                int endItemIndex = Math.Min(startItemIndex + itemsPerPage, matchedItems.Count);

                using (var dbContext = new MyDbContext())
                {
                    if (oldIndex >= startItemIndex && oldIndex < endItemIndex)
                    {
                        var oldItem = matchedItems[oldIndex];
                        var oldCategory = dbContext.Categories.FirstOrDefault(c => c.Id == oldItem.CategoryId)?.Name ?? "Unknown";
                        Console.SetCursorPosition(0, oldIndex - startItemIndex + itemStartLine);
                        if (oldItem.Stock == 0)
                        {
                            Console.WriteLine("{0,-35} {1,12} {2,15} {3,15} {4,19}", oldItem.Name, oldCategory, oldItem.Level, oldItem.Rarity, "   Out of stock");
                        }
                        else
                        {
                            Console.WriteLine("{0,-35} {1,12} {2,15} {3,15} {4,14} gold", oldItem.Name, oldCategory, oldItem.Level, oldItem.Rarity, oldItem.Price);
                        }
                    }

                    if (newIndex >= startItemIndex && newIndex < endItemIndex)
                    {
                        var newItem = matchedItems[newIndex];
                        var newCategory = dbContext.Categories.FirstOrDefault(c => c.Id == newItem.CategoryId)?.Name ?? "Unknown";
                        Console.SetCursorPosition(0, newIndex - startItemIndex + itemStartLine);
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                        if (newItem.Stock == 0)
                        {
                            Console.WriteLine("{0,-35} {1,12} {2,15} {3,15} {4,19}", newItem.Name, newCategory, newItem.Level, newItem.Rarity, "   Out of stock");
                        }
                        else
                        {
                            Console.WriteLine("{0,-35} {1,12} {2,15} {3,15} {4,14} gold", newItem.Name, newCategory, newItem.Level, newItem.Rarity, newItem.Price);
                        }
                        Console.ResetColor();
                    }
                }
            }

            do
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                int oldIndex = selectedIndex;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    selectedIndex--;
                    if (selectedIndex < currentPage * itemsPerPage)
                    {
                        selectedIndex = Math.Min((currentPage + 1) * itemsPerPage, matchedItems.Count) - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    selectedIndex++;
                    if (selectedIndex >= Math.Min((currentPage + 1) * itemsPerPage, matchedItems.Count))
                    {
                        selectedIndex = currentPage * itemsPerPage;
                    }
                }
                else if (keyPressed == ConsoleKey.LeftArrow && totalPages > 1)
                {
                    currentPage--;
                    if (currentPage < 0)
                    {
                        currentPage = totalPages - 1;
                    }
                    selectedIndex = currentPage * itemsPerPage;
                    DisplayPage();
                }
                else if (keyPressed == ConsoleKey.RightArrow && totalPages > 1)
                {
                    currentPage++;
                    if (currentPage >= totalPages)
                    {
                        currentPage = 0;
                    }
                    selectedIndex = currentPage * itemsPerPage;
                    DisplayPage();
                }
                else if (keyPressed == ConsoleKey.Enter)
                {
                    var selectedItem = matchedItems[selectedIndex];
                    IncrementProductClick(selectedItem);
                    ItemDetails.DisplayItemDetails(selectedItem, mainMenu, null);
                    NavigateMatchedItems(matchedItems, mainMenu, selectedIndex);
                    return;
                }
                else if (keyPressed == ConsoleKey.B)
                {
                    var selectedItem = matchedItems[selectedIndex];
                    if (selectedItem.Stock == 0)
                    {
                        Console.SetCursorPosition(0, itemStartLine + itemsPerPage + 6);
                        Console.WriteLine("This item is out of stock and cannot be added to the cart.");
                        System.Threading.Thread.Sleep(2000);
                        DisplayPage();
                    }
                    else
                    {
                        Cart.AddToCart(selectedItem);
                        Console.SetCursorPosition(0, itemStartLine + itemsPerPage + 6);
                        Console.WriteLine("Item added to cart.");
                        System.Threading.Thread.Sleep(1000);
                        DisplayPage();
                    }
                }
                else if (keyPressed == ConsoleKey.Escape)
                {
                    return;
                }

                if (oldIndex != selectedIndex)
                {
                    UpdateSelection(oldIndex, selectedIndex);
                }
            } while (true);
        }

        private static void IncrementProductClick(Product product)
        {
            using (var dbContext = new MyDbContext())
            {
                var dbProduct = dbContext.Products.FirstOrDefault(p => p.Id == product.Id);
                if (dbProduct != null)
                {
                    dbProduct.Click = (dbProduct.Click ?? 0) + 1;
                    dbContext.SaveChanges();
                }
            }
        }
    }
}
