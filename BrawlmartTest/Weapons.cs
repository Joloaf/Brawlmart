using BrawlmartTest.Models;

namespace BrawlmartTest
{
    public class Weapons
    {
        private List<Product> products;

        public Weapons()
        {
            LoadProducts();
        }

        private void LoadProducts()
        {
            using (var dbContext = new MyDbContext())
            {
                products = dbContext.Products
                    .Where(p => p.CategoryId == 1)
                    .OrderBy(p => p.Name)
                    .ToList();
            }
        }

        public void ReloadProducts()
        {
            LoadProducts();
        }

        public void NavigateProducts(Menu mainMenu, Menu subMenu, int initialIndex = 0)
        {
            int selectedIndex = initialIndex;
            int currentPage = 0;
            int itemsPerPage = 15;
            int totalPages = (int)Math.Ceiling((double)products.Count / itemsPerPage);
            int itemStartLine = 15;
            ConsoleKey keyPressed;

            void DisplayPage()
            {
                Console.Clear();
                Title.DisplayTitle();
                mainMenu.DisplayOptions();
                Console.WriteLine();
                subMenu.DisplayOptions();
                Console.WriteLine();

                Console.WriteLine();
                Console.WriteLine("{0,-35} {1,12} {2,15} {3,15} {4,19}", "Name", "Category", "Level", "Rarity", "Price");
                Console.WriteLine(new string('-', 100));

                int startItemIndex = currentPage * itemsPerPage;
                int endItemIndex = Math.Min(startItemIndex + itemsPerPage, products.Count);

                using (var dbContext = new MyDbContext())
                {
                    for (int i = startItemIndex; i < endItemIndex; i++)
                    {
                        var item = products[i];
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
                int endItemIndex = Math.Min(startItemIndex + itemsPerPage, products.Count);

                using (var dbContext = new MyDbContext())
                {
                    if (oldIndex >= startItemIndex && oldIndex < endItemIndex)
                    {
                        var oldItem = products[oldIndex];
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
                        var newItem = products[newIndex];
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
                        selectedIndex = Math.Min((currentPage + 1) * itemsPerPage, products.Count) - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    selectedIndex++;
                    if (selectedIndex >= Math.Min((currentPage + 1) * itemsPerPage, products.Count))
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
                    var selectedItem = products[selectedIndex];
                    IncrementProductClick(selectedItem);
                    ItemDetails.DisplayItemDetails(selectedItem, mainMenu, null);
                    NavigateProducts(mainMenu, subMenu, selectedIndex);
                    return;
                }
                else if (keyPressed == ConsoleKey.B)
                {
                    var selectedItem = products[selectedIndex];
                    if (selectedItem.Stock == 0)
                    {
                        Console.SetCursorPosition(0, itemStartLine + itemsPerPage + 3);
                        Console.WriteLine("This item is out of stock and cannot be added to the cart.");
                        System.Threading.Thread.Sleep(2000);
                        DisplayPage();
                    }
                    else
                    {
                        Console.SetCursorPosition(0, itemStartLine + itemsPerPage + 3);
                        Cart.AddToCartAsync(selectedItem);
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

        private void IncrementProductClick(Product product)
        {
            using (var dbContext = new MyDbContext())
            {
                var dbProduct = dbContext.Products.FirstOrDefault(p => p.Id == product.Id);
                if (dbProduct != null)
                {
                    dbProduct.Click = (dbProduct.Click ?? 0) + 1;
                    dbContext.SaveChanges();

                    Models.MongoData mongoData = new Models.MongoData()
                    {
                        Id = Guid.NewGuid(),
                        ProductName = dbProduct.Name,
                        UserInterest = dbProduct.Click ?? 0
                    };
                }
            }
        }
    }
}