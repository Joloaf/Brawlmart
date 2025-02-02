using static System.Console;
using BrawlmartTest.Models;

namespace BrawlmartTest
{
    public class Structure
    {
        public Weapons weapons;
        public Armors armors;
        public Trinkets trinkets;
        public List<Product> frontPageItems;
        public Menu mainMenu;
        public User currentUser { get; private set; }
        private bool isDisplayingCart;

        public Structure()
        {
            weapons = new Weapons();
            armors = new Armors();
            trinkets = new Trinkets();
            LoadFrontPageItems();

            // Update the Cart counter in the main menu when increasing or decreasing quantity of an item in the cart
            Cart.ItemAdded += () => UpdateMainMenu(mainMenu);
            Cart.ItemRemoved += () => UpdateMainMenu(mainMenu);
        }

        private void LoadFrontPageItems()
        {
            using (var dbContext = new MyDbContext())
            {
                frontPageItems = dbContext.Products.Where(p => p.FrontId.HasValue).OrderBy(p => p.FrontId).ToList();
            }
        }

        public void ReloadFrontPageItems()
        {
            LoadFrontPageItems();
        }

        public void PageStructure()
        {
            RunMainMenu();
        }

        public void RunMainMenu()
        {
            string[] mainOptions = { "Marketplace", "Search Loot", "Loot Wagon", "Character Info", "Exit Village" };
            mainMenu = new Menu(mainOptions);
            int selectedIndex = 0;
            bool inMainMenu = true;

            do
            {
                Clear();
                Title.DisplayTitle();
                CursorVisible = false;
                UpdateMainMenu(mainMenu);
                mainMenu.DisplayOptions();
                Console.WriteLine();
                DisplayFrontPageItems(selectedIndex, inMainMenu);

                ConsoleKeyInfo keyInfo = ReadKey(true);
                ConsoleKey keyPressed = keyInfo.Key;

                if (inMainMenu)
                {
                    if (keyPressed == ConsoleKey.Enter)
                    {
                        selectedIndex = mainMenu.SelectedIndex;
                        switch (selectedIndex)
                        {
                            case 0:
                                MarketPlace(mainMenu);
                                break;
                            case 1:
                                SearchLoot(mainMenu);
                                break;
                            case 2:
                                LootWagon(mainMenu);
                                break;
                            case 3:
                                CharacterInfo(mainMenu);
                                break;
                            case 4:
                                ExitVillage();
                                break;
                        }
                    }
                    else if (keyPressed == ConsoleKey.DownArrow)
                    {
                        inMainMenu = false;
                        selectedIndex = 0;
                    }
                    else if (!isDisplayingCart)
                    {
                        if (keyPressed == ConsoleKey.LeftArrow)
                        {
                            mainMenu.SelectedIndex--;
                            if (mainMenu.SelectedIndex < 0)
                            {
                                mainMenu.SelectedIndex = mainOptions.Length - 1;
                            }
                        }
                        else if (keyPressed == ConsoleKey.RightArrow)
                        {
                            mainMenu.SelectedIndex++;
                            if (mainMenu.SelectedIndex >= mainOptions.Length)
                            {
                                mainMenu.SelectedIndex = 0;
                            }
                        }
                    }
                }
                else
                {
                    if (keyPressed == ConsoleKey.UpArrow)
                    {
                        if (selectedIndex == 0)
                        {
                            inMainMenu = true;
                        }
                        else
                        {
                            selectedIndex--;
                        }
                    }
                    else if (keyPressed == ConsoleKey.Enter)
                    {
                        HandleFrontPageItemSelection(selectedIndex, mainMenu);
                    }
                    else if (keyPressed == ConsoleKey.DownArrow)
                    {
                        selectedIndex++;
                        if (selectedIndex >= frontPageItems.Count)
                        {
                            selectedIndex = 0;
                        }
                    }
                }
            } while (true);
        }

        internal void UpdateMainMenu(Menu mainMenu)
        {
            int cartCount = Cart.GetItemCount();
            if (cartCount > 0)
            {
                mainMenu.UpdateOption(2, $"Loot Wagon ({cartCount})");
            }
            else
            {
                mainMenu.UpdateOption(2, "Loot Wagon");
            }

            string userNameOption = currentUser != null ? $"  {currentUser.UserName}  " : "Character Info";
            mainMenu.UpdateOption(3, userNameOption);
        }

        private void DisplayFrontPageItems(int selectedIndex, bool inMainMenu)
        {
            Console.WriteLine();
            Console.WriteLine("Featured Items:");
            Console.WriteLine("---------------");

            for (int i = 0; i < frontPageItems.Count; i++)
            {
                var item = frontPageItems[i];
                if (!inMainMenu && i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                Console.WriteLine($"{i + 1}. {item.Name}");
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine("Use the arrow keys to navigate and Enter to select an option...");
        }

        private void HandleFrontPageItemSelection(int index, Menu mainMenu)
        {
            if (index >= 0 && index < frontPageItems.Count)
            {
                var selectedProduct = frontPageItems[index];
                IncrementProductClick(selectedProduct);
                string[] marketOptions = { "Weapons", "Armors", "Trinkets" };
                Menu marketMenu = new Menu(marketOptions);
                ItemDetails.DisplayItemDetails(selectedProduct, mainMenu, marketMenu);
            }
        }

        private void MarketPlace(Menu mainMenu)
        {
            using (var dbContext = new MyDbContext())
            {
                var categories = dbContext.Categories.ToList();
                string[] marketOptions = categories.Select(c => c.Name).ToArray();
                Menu marketMenu = new Menu(marketOptions);

                int selectedIndex = 0;
                do
                {
                    Clear();
                    Title.DisplayTitle();
                    mainMenu.DisplayOptions();
                    WriteLine();
                    marketMenu.DisplayOptions();

                    ConsoleKeyInfo keyInfo = ReadKey(true);
                    ConsoleKey keyPressed = keyInfo.Key;

                    if (keyPressed == ConsoleKey.Escape)
                    {
                        return;
                    }
                    else if (keyPressed == ConsoleKey.LeftArrow)
                    {
                        marketMenu.SelectedIndex--;
                        if (marketMenu.SelectedIndex == -1)
                        {
                            marketMenu.SelectedIndex = marketOptions.Length - 1;
                        }
                    }
                    else if (keyPressed == ConsoleKey.RightArrow)
                    {
                        marketMenu.SelectedIndex++;
                        if (marketMenu.SelectedIndex == marketOptions.Length)
                        {
                            marketMenu.SelectedIndex = 0;
                        }
                    }
                    else if (keyPressed == ConsoleKey.Enter)
                    {
                        selectedIndex = marketMenu.SelectedIndex;
                        if (selectedIndex == 0)
                        {
                            weapons.NavigateProducts(mainMenu, marketMenu);
                        }
                        else if (selectedIndex == 1)
                        {
                            armors.NavigateProducts(mainMenu, marketMenu);
                        }
                        else if (selectedIndex == 2)
                        {
                            trinkets.NavigateProducts(mainMenu, marketMenu);
                        }
                    }
                } while (true);
            }
        }

        public void LootWagon(Menu mainMenu)
        {
            bool isLoggedIn = currentUser != null;
            Checkout.User checkoutUser = MapUserToCheckoutUser(currentUser);
            isDisplayingCart = true;
            Cart.DisplayCartAsync(this, mainMenu, isLoggedIn, checkoutUser).ContinueWith(_ => isDisplayingCart = false); // Reset the flag after DisplayCartAsync completes
        }

        private Checkout.User MapUserToCheckoutUser(User user)
        {
            if (user == null) return null;

            return new Checkout.User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                StreetAddress = user.StreetAddress,
                PostalCode = user.PostalCode,
                City = user.City,
                Country = user.Country
            };
        }

        private void SearchLoot(Menu mainMenu)
        {
            SearchItem.Search(mainMenu);
        }

        private void CharacterInfo(Menu mainMenu)
        {
            if (currentUser != null)
            {
                if (currentUser.Admin)
                {
                    Admin.DisplayAdminPage(currentUser, this, mainMenu);
                }
                else
                {
                    CharacterSheet.DisplayCharacterSheet(currentUser, this, mainMenu);
                }
            }
            else
            {
                DisplayLoginOptions(mainMenu);
            }
        }

        private void DisplayLoginOptions(Menu mainMenu)
        {
            string[] loginOptions = { "Login", "Create Account" };
            Menu loginMenu = new Menu(loginOptions);

            int selectedIndex = 0;
            do
            {
                Clear();
                Title.DisplayTitle();
                mainMenu.DisplayOptions();
                WriteLine();
                loginMenu.DisplayOptions();

                ConsoleKeyInfo keyInfo = ReadKey(true);
                ConsoleKey keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.Escape)
                {
                    return;
                }
                else if (keyPressed == ConsoleKey.LeftArrow)
                {
                    loginMenu.SelectedIndex--;
                    if (loginMenu.SelectedIndex == -1)
                    {
                        loginMenu.SelectedIndex = loginOptions.Length - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.RightArrow)
                {
                    loginMenu.SelectedIndex++;
                    if (loginMenu.SelectedIndex == loginOptions.Length)
                    {
                        loginMenu.SelectedIndex = 0;
                    }
                }
                else if (keyPressed == ConsoleKey.Enter)
                {
                    selectedIndex = loginMenu.SelectedIndex;
                    if (selectedIndex == 0)
                    {
                        Login.DisplayLogin(this, mainMenu);
                    }
                    else if (selectedIndex == 1)
                    {
                        CreateAccount.DisplayCreateAccount(this, mainMenu);
                    }
                }
            } while (true);
        }

        public void SetCurrentUser(User user)
        {
            currentUser = user;
        }

        private void ExitVillage()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@"
                                                                                                __ 
              _____ _           _                                                         _    |  |
             |_   _| |_ ___ ___| |_    _ _ ___ _ _       ___ ___ _____ ___    ___ ___ ___|_|___|  |
               | | |   | .'|   | '_|  | | | . | | | _   |  _| . |     | -_|  | .'| . | .'| |   |__|
               |_| |_|_|__,|_|_|_,_|  |_  |___|___|| |  |___|___|_|_|_|___|  |__,|_  |__,|_|_|_|__|
                                      |___|        |_|                           |___|             ");
            System.Threading.Thread.Sleep(3000);
            Console.ResetColor();
            Environment.Exit(0);
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
                }
            }
        }
    }
}