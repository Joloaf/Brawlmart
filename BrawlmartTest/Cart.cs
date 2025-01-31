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

        public static void AddToCart(Product product)
        {
            var existingItem = cartItems.FirstOrDefault(item => item.Product.Id == product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cartItems.Add(new CartItem { Product = product, Quantity = 1 });
            }
            ItemAdded?.Invoke();
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

        public static void DisplayCart(Menu mainMenu, bool isLoggedIn, Checkout.User currentUser)
        {
            int selectedIndex = 0;
            ConsoleKey keyPressed;

            do
            {
                Console.Clear();
                Title.DisplayTitle();
                mainMenu.DisplayOptions();
                Console.WriteLine();

                if (cartItems.Count == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Your cart is empty.");
                    Console.WriteLine("Press any key to return to the main menu...");

                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    keyPressed = keyInfo.Key;

                    if (keyPressed == ConsoleKey.LeftArrow || keyPressed == ConsoleKey.RightArrow || keyPressed == ConsoleKey.Escape)
                    {
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Your Loot Wagon... Pleaese proceed to the nearest checkout counter!");
                    Console.WriteLine();
                    Console.WriteLine("{0,-32} {1,40} {2,26}", "Name", "Quantity", "Price");
                    Console.WriteLine("----------------------------------------------------------------------------------------------------");

                    float totalPrice = 0;

                    for (int i = 0; i < cartItems.Count; i++)
                    {
                        var cartItem = cartItems[i];
                        var product = cartItem.Product;
                        if (i == selectedIndex)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;
                        }
                        float itemTotalPrice = (product.Price ?? 0) * cartItem.Quantity;
                        totalPrice += itemTotalPrice;
                        Console.WriteLine("{0,-32} {1,40} {2,21} gold", product.Name, cartItem.Quantity, itemTotalPrice);
                        Console.ResetColor();
                    }

                    Console.WriteLine("====================================================================================================");
                    Console.WriteLine("{0,86} {1,8} gold", "Total:", totalPrice);
                    Console.WriteLine();
                    Console.WriteLine("Use the arrow keys to navigate and adjust quantity...");
                    Console.WriteLine("Press X to remove an item from your cart...");
                    Console.WriteLine("Press C to checkout...");
                    Console.WriteLine("Press Esc to return to the main menu...");

                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    keyPressed = keyInfo.Key;

                    if (keyPressed == ConsoleKey.UpArrow)
                    {
                        selectedIndex--;
                        if (selectedIndex < 0)
                        {
                            selectedIndex = cartItems.Count - 1;
                        }
                    }
                    else if (keyPressed == ConsoleKey.DownArrow)
                    {
                        selectedIndex++;
                        if (selectedIndex >= cartItems.Count)
                        {
                            selectedIndex = 0;
                        }
                    }
                    else if (keyPressed == ConsoleKey.RightArrow && cartItems.Count > 0)
                    {
                        cartItems[selectedIndex].Quantity++;
                        ItemAdded?.Invoke();
                    }
                    else if (keyPressed == ConsoleKey.LeftArrow && cartItems.Count > 0)
                    {
                        if (cartItems[selectedIndex].Quantity > 1)
                        {
                            cartItems[selectedIndex].Quantity--;
                            ItemRemoved?.Invoke();
                        }
                    }
                    else if (keyPressed == ConsoleKey.X && cartItems.Count > 0)
                    {
                        Console.WriteLine("Are you sure you want to remove this item? (Y/N)");
                        ConsoleKeyInfo confirmKey = Console.ReadKey(true);
                        if (confirmKey.Key == ConsoleKey.Y)
                        {
                            RemoveFromCart(selectedIndex);
                            if (selectedIndex >= cartItems.Count)
                            {
                                selectedIndex = cartItems.Count - 1;
                            }
                        }
                    }
                    else if (keyPressed == ConsoleKey.C)
                    {
                        if (!isLoggedIn)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Please log in or create an account by selecting Character Info in the menu before trying to checkout.");
                            Console.WriteLine("Press any key to return to the main menu...");
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
            } while (true);
        }

        //public static List<string> GetItemNames()
        //{
        //    return cartItems.Select(item => item.Product.Name).ToList();
        //}

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
