using BrawlmartTest.Models;
using System.Text;

namespace BrawlmartTest
{
    public static class Checkout
    {
        private static User currentUser;

        public class User
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string StreetAddress { get; set; }
            public string PostalCode { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
        }

        public class CartItemDetail
        {
            public string ItemName { get; set; }
            public int Quantity { get; set; }
            public float Price { get; set; }

        }

        public static void StartCheckout(Menu mainMenu, float totalPrice, bool isLoggedIn, User currentUser)
        {
            string[] addressFields = { "First Name", "Last Name", "Email", "Phone Number", "Street Address", "Postal Code", "City", "Country" };
            string[] addressValues = new string[addressFields.Length];
            int selectedIndex = 0;

            if (isLoggedIn && currentUser != null)
            {
                addressValues[0] = currentUser.FirstName;
                addressValues[1] = currentUser.LastName;
                addressValues[2] = currentUser.Email;
                addressValues[3] = currentUser.PhoneNumber;
                addressValues[4] = currentUser.StreetAddress;
                addressValues[5] = currentUser.PostalCode;
                addressValues[6] = currentUser.City;
                addressValues[7] = currentUser.Country;
            }

            ConsoleKey keyPressed;
            do
            {
                Console.Clear();
                Title.DisplayTitle();
                mainMenu.DisplayOptions();
                Console.WriteLine();
                Console.WriteLine("Customer Address:");
                Console.WriteLine(new string('-', 100));
                for (int i = 0; i < addressFields.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine($"{addressFields[i]}: {addressValues[i]}");
                    Console.ResetColor();
                }
                Console.WriteLine();
                Console.WriteLine("Use the arrow keys to navigate and Enter to edit a field...");
                Console.WriteLine("Press C to proceed with the checkout...");
                Console.WriteLine("Press Esc to return to the main menu...");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    selectedIndex--;
                    if (selectedIndex < 0)
                    {
                        selectedIndex = addressFields.Length - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    selectedIndex++;
                    if (selectedIndex >= addressFields.Length)
                    {
                        selectedIndex = 0;
                    }
                }
                else if (keyPressed == ConsoleKey.Enter)
                {
                    Console.SetCursorPosition(addressFields[selectedIndex].Length + 2, selectedIndex + 12);
                    Console.Write(addressValues[selectedIndex]);
                    Console.SetCursorPosition(addressFields[selectedIndex].Length + 2 + addressValues[selectedIndex].Length, selectedIndex + 12);
                    StringBuilder input = new StringBuilder(addressValues[selectedIndex]);
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
                            if (input.Length > 0)
                            {
                                input.Remove(input.Length - 1, 1);
                                Console.SetCursorPosition(addressFields[selectedIndex].Length + 2, selectedIndex + 12);
                                Console.Write(new string(' ', addressValues[selectedIndex].Length + 1));
                                Console.SetCursorPosition(addressFields[selectedIndex].Length + 2, selectedIndex + 12);
                                Console.Write(input.ToString());
                            }
                        }
                        else if (!char.IsControl(keyInfo.KeyChar))
                        {
                            input.Append(keyInfo.KeyChar);
                            Console.Write(keyInfo.KeyChar);
                        }
                    }
                    addressValues[selectedIndex] = input.ToString();
                }
                else if (keyPressed == ConsoleKey.C)
                {
                    if (!isLoggedIn)
                    {
                        Console.WriteLine("Please log in or create an account before trying to checkout again.");
                        Console.ReadKey(true);
                        return;
                    }

                    if (addressValues.All(value => !string.IsNullOrEmpty(value)))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("All address fields must be filled out.");
                        Console.ReadKey(true);
                    }
                }
                else if (keyPressed == ConsoleKey.Escape)
                {
                    Console.WriteLine("Checkout canceled...");
                    return;
                }
            } while (true);

            string[] deliveryOptions = { "[UPS]Unicorn Parcel Service (1-2 days delivery time : +500 gold)", "PostHorde (10-50 days delivery time : Free, at your own risk...)" };
            int deliveryIndex = 0;
            do
            {
                Console.Clear();
                Title.DisplayTitle();
                mainMenu.DisplayOptions();
                Console.WriteLine();
                Console.WriteLine("Select a delivery option:");
                Console.WriteLine(new string('-', 100));
                for (int i = 0; i < deliveryOptions.Length; i++)
                {
                    if (i == deliveryIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine(deliveryOptions[i]);
                    Console.ResetColor();
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    deliveryIndex--;
                    if (deliveryIndex < 0)
                    {
                        deliveryIndex = deliveryOptions.Length - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    deliveryIndex++;
                    if (deliveryIndex >= deliveryOptions.Length)
                    {
                        deliveryIndex = 0;
                    }
                }
                else if (keyPressed == ConsoleKey.Enter)
                {
                    break;
                }
                else if (keyPressed == ConsoleKey.Escape)
                {
                    Console.WriteLine();
                    Console.WriteLine("Checkout canceled.");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }
            } while (true);

            if (deliveryIndex == 0)
            {
                totalPrice += 500;
                Console.WriteLine();
                Console.WriteLine($"Delivery by [UPS] Unicorn Parcel Service selected. Smart choice!");
                System.Threading.Thread.Sleep(2000);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Delivery by PostHorde selected. Godspeed!");
                System.Threading.Thread.Sleep(2000);
            }

            string[] paymentOptions = { "CreditBard", "PayPaladin", "I-Owe-You-Notes" };
            int paymentIndex = 0;
            do
            {
                Console.Clear();
                Title.DisplayTitle();
                mainMenu.DisplayOptions();
                Console.WriteLine();
                Console.WriteLine("Select a payment method:");
                Console.WriteLine(new string('-', 100));
                for (int i = 0; i < paymentOptions.Length; i++)
                {
                    if (i == paymentIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine(paymentOptions[i]);
                    Console.ResetColor();
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    paymentIndex--;
                    if (paymentIndex < 0)
                    {
                        paymentIndex = paymentOptions.Length - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    paymentIndex++;
                    if (paymentIndex >= paymentOptions.Length)
                    {
                        paymentIndex = 0;
                    }
                }
                else if (keyPressed == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    Console.WriteLine("Payment method selected...");
                    System.Threading.Thread.Sleep(1000);
                    break;
                }
                else if (keyPressed == ConsoleKey.Escape)
                {
                    Console.WriteLine();
                    Console.WriteLine("Checkout canceled...");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }
            } while (true);

            List<CartItemDetail> cartItems = Cart.GetCartItems();

            Console.Clear();
            Title.DisplayTitle();
            mainMenu.DisplayOptions();
            Console.WriteLine();
            Console.WriteLine("Order Summary:");
            Console.WriteLine(new string('-', 100));
            Console.WriteLine("Items Ordered:");
            foreach (var item in cartItems)
            {
                Console.WriteLine($"{item.ItemName} - Quantity: {item.Quantity}, Price: {item.Price} gold");
            }
            Console.WriteLine();
            Console.WriteLine("Shipping Address:");
            for (int i = 0; i < addressFields.Length; i++)
            {
                Console.WriteLine($"{addressFields[i]}: {addressValues[i]}");
            }
            Console.WriteLine();
            Console.WriteLine($"Postal Service: {deliveryOptions[deliveryIndex]}");
            Console.WriteLine($"Payment Method: {paymentOptions[paymentIndex]}");
            Console.WriteLine($"Total Price: {totalPrice} gold");
            Console.WriteLine();
            Console.WriteLine("Press Y to confirm the order or N to cancel the order.");
            Console.WriteLine();

            do
            {
                keyPressed = Console.ReadKey(true).Key;
                if (keyPressed == ConsoleKey.Y)
                {
                    Console.WriteLine("Order confirmed. Thank you for your purchase!");
                    SaveOrder(currentUser, totalPrice, cartItems);
                    System.Threading.Thread.Sleep(1000);
                    DisplayThankYouMessage(mainMenu);
                    break;
                }
                else if (keyPressed == ConsoleKey.N)
                {
                    Console.WriteLine("Order canceled...");
                    System.Threading.Thread.Sleep(1000);
                    break;
                }
                else if (keyPressed == ConsoleKey.Escape)
                {
                    Console.WriteLine("Checkout canceled...");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }
            } while (keyPressed != ConsoleKey.Y && keyPressed != ConsoleKey.N);
        }

        private static void SaveOrder(Checkout.User currentUser, float totalPrice, List<CartItemDetail> cartItems)
        {
            using (var context = new MyDbContext())
            {
                try
                {
                    var userInDb = context.Users.FirstOrDefault(u => u.Email == currentUser.Email);
                    if (userInDb == null)
                    {
                        // THIS IS STILL NOT WORKING AS INTENDED AND I'LL GET BACK TO FIXING THIS LATER, I PROMISE...
                        // Create a new user... or so I wish...
                        var newUser = new BrawlmartTest.Models.User
                        {
                            FirstName = currentUser.FirstName,
                            LastName = currentUser.LastName,
                            Email = currentUser.Email,
                            PhoneNumber = currentUser.PhoneNumber,
                            StreetAddress = currentUser.StreetAddress,
                            PostalCode = currentUser.PostalCode,
                            City = currentUser.City,
                            Country = currentUser.Country
                        };
                        context.Users.Add(newUser);
                        context.SaveChanges();
                        userInDb = newUser;
                    }

                    // Create a new order
                    var order = new Order
                    {
                        Date = DateTime.Now,
                        TotalPrice = totalPrice,
                        UserId = userInDb.Id,
                        OrderProducts = new List<OrderProduct>()
                    };
                    context.Orders.Add(order);
                    context.SaveChanges();

                    foreach (var item in cartItems)
                    {
                        var product = context.Products.FirstOrDefault(p => p.Name == item.ItemName);
                        if (product == null)
                        {
                            Console.WriteLine($"Product not found: {item.ItemName}");
                            System.Threading.Thread.Sleep(2000);
                            continue;
                        }

                        if (product.Stock < item.Quantity)
                        {
                            Console.WriteLine($"Not enough stock for product: {product.Name}");
                            System.Threading.Thread.Sleep(2000);
                            continue;
                        }

                        var priceFromDb = product.Price ?? 0;
                        var calculatedPrice = item.Quantity * priceFromDb;

                        var orderProduct = new OrderProduct
                        {
                            OrderId = order.Id,
                            ProductId = product.Id,
                            Quantity = item.Quantity,
                            Price = calculatedPrice
                        };
                        context.OrderProducts.Add(orderProduct);
                        product.Stock -= item.Quantity;
                        context.Products.Update(product);
                    }
                    context.SaveChanges();
                    Cart.EmptyCart();
                    Console.WriteLine("Order saved successfully.");
                    System.Threading.Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving order: {ex.Message}");
                }
            }
        }
        public static void DisplayThankYouMessage(Menu mainMenu)
        {
            Console.Clear();
            Title.DisplayTitle();
            mainMenu.DisplayOptions();
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@"
                                                              __ 
                         _____ _           _                 |  |
                        |_   _| |_ ___ ___| |_    _ _ ___ _ _|  |
                          | | |   | .'|   | '_|  | | | . | | |__|
                          |_| |_|_|__,|_|_|_,_|  |_  |___|___|__|
                                                 |___|           ");
            Console.ResetColor();
            System.Threading.Thread.Sleep(3000);
            Cart.EmptyCart();
        }
    }
}