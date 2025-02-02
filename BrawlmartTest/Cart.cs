using BrawlmartTest.Models;

namespace BrawlmartTest
{
    public static class Cart
    {
        private static List<CartItem> cartItems = new List<CartItem>();

        public static List<Checkout.CartItemDetail> GetCartItems()
        {
            return cartItems.Select(item => new Checkout.CartItemDetail
            {
                ItemName = item.Product.Name,
                Quantity = item.Quantity,
                Price = (item.Product.Price ?? 0) * item.Quantity
            }).ToList();
        }

        // Define events to notify when an item is added or removed from the cart
        public static event Action ItemAdded;
        public static event Action ItemRemoved;

        public static async Task AddToCartAsync(Product product)
        {
            using (var context = new MyDbContext())
            {
                var productInDb = await context.Products.FindAsync(product.Id);
                if (productInDb == null)
                {
                    Console.WriteLine("Product not found.");
                    return;
                }

                int stock = productInDb.Stock ?? 0;
                var existingItem = cartItems.FirstOrDefault(item => item.Product.Id == product.Id);
                int cartQuantity = existingItem != null ? existingItem.Quantity : 0;

                if (cartQuantity + 1 > stock)
                {
                    Console.WriteLine("Cannot add more of this product. Stock limit reached.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey(true);
                    return;
                }

                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    cartItems.Add(new CartItem { Product = productInDb, Quantity = 1 });
                }
                ItemAdded?.Invoke();
            }
        }

        public static async Task DisplayCartAsync(Structure structure, Menu mainMenu, bool isLoggedIn, Checkout.User currentUser)
        {
            int selectedIndex = 0;
            ConsoleKey keyPressed;

            structure.UpdateMainMenu(mainMenu); // Update main menu cart value initially

            if (!cartItems.Any())
            {
                Console.Clear();
                Title.DisplayTitle();
                mainMenu.DisplayOptions();
                Console.SetCursorPosition(0, 9);
                Console.WriteLine("\nYour Loot Wagon is empty. Please add items to your cart.");
                Console.WriteLine("Press any key to return to the main menu...");
                Console.ReadKey(true);
                return;
            }

            while (true)  // Keep the loop running without restarting
            {
                Console.Clear();
                Title.DisplayTitle();
                mainMenu.DisplayOptions(); // Display the main menu under the title

                Console.SetCursorPosition(0, 9); // Reset cursor instead of clearing screen
                Console.WriteLine("\nYour Loot Wagon... Please proceed to the nearest checkout counter!\n");
                Console.WriteLine("{0,-32} {1,40} {2,26}", "Name", "Quantity", "Price");
                Console.WriteLine("----------------------------------------------------------------------------------------------------");

                float totalPrice = 0;

                for (int i = 0; i < cartItems.Count; i++)
                {
                    var cartItem = cartItems[i];
                    var product = cartItem.Product;
                    float itemTotalPrice = (product.Price ?? 0) * cartItem.Quantity;
                    totalPrice += itemTotalPrice;

                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                    }

                    Console.WriteLine("{0,-32} {1,40} {2,21} gold", product.Name, cartItem.Quantity, itemTotalPrice);
                    Console.ResetColor();
                }

                Console.WriteLine("====================================================================================================");
                Console.WriteLine("{0,86} {1,8} gold", "Total:", totalPrice);
                Console.WriteLine();
                Console.WriteLine("Use arrow keys to navigate and adjust quantity...");
                Console.WriteLine("Press X to remove an item from your cart...");
                Console.WriteLine("Press C to checkout...");
                Console.WriteLine("Press Esc to return to the main menu...");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    selectedIndex = (selectedIndex - 1 + cartItems.Count) % cartItems.Count;
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    selectedIndex = (selectedIndex + 1) % cartItems.Count;
                }
                else if (keyPressed == ConsoleKey.RightArrow && cartItems.Count > 0)
                {
                    int stock = cartItems[selectedIndex].Product.Stock ?? 0;
                    if (cartItems[selectedIndex].Quantity < stock)
                    {
                        cartItems[selectedIndex].Quantity++;
                        ItemAdded?.Invoke();
                        structure.UpdateMainMenu(mainMenu); // Update main menu cart value
                    }
                }
                else if (keyPressed == ConsoleKey.LeftArrow && cartItems.Count > 0)
                {
                    if (cartItems[selectedIndex].Quantity > 1)
                    {
                        cartItems[selectedIndex].Quantity--;
                        ItemRemoved?.Invoke();
                        structure.UpdateMainMenu(mainMenu); // Update main menu cart value
                    }
                }
                else if (keyPressed == ConsoleKey.X && cartItems.Count > 0)
                {
                    Console.WriteLine("Are you sure you want to remove this item? (Y/N)");
                    if (Console.ReadKey(true).Key == ConsoleKey.Y)
                    {
                        RemoveFromCart(selectedIndex);
                        selectedIndex = Math.Max(0, selectedIndex - 1);
                        structure.UpdateMainMenu(mainMenu); // Update main menu cart value

                        if (!cartItems.Any())
                        {
                            Console.Clear();
                            Title.DisplayTitle();
                            mainMenu.DisplayOptions();
                            Console.SetCursorPosition(0, 9);
                            Console.WriteLine("\nYour Loot Wagon is empty. Please add items to your cart.");
                            Console.WriteLine("Press any key to return to the main menu...");
                            Console.ReadKey(true);
                            return;
                        }
                    }
                }
                else if (keyPressed == ConsoleKey.C)
                {
                    if (!isLoggedIn)
                    {
                        Console.WriteLine("\nPlease log in before checking out.");
                        Console.WriteLine("Press any key to return...");
                        Console.ReadKey(true);
                        return;
                    }
                    Checkout.StartCheckout(mainMenu, totalPrice, isLoggedIn, currentUser);
                }
                else if (keyPressed == ConsoleKey.Escape)
                {
                    return;
                }
            }
        }



        public static void RemoveFromCart(int index)
        {
            if (index >= 0 && index < cartItems.Count)
            {
                cartItems.RemoveAt(index);
                ItemRemoved?.Invoke();
            }
        }

        public static int GetItemCount()
        {
            return cartItems.Sum(item => item.Quantity);
        }

        public static void EmptyCart()
        {
            cartItems.Clear();
            ItemRemoved?.Invoke();
        }

        private class CartItem
        {
            public Product Product { get; set; }
            public int Quantity { get; set; }
        }
    }
}
