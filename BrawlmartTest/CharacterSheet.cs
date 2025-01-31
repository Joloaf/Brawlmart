using BrawlmartTest.Models;

namespace BrawlmartTest
{
    internal static class CharacterSheet
    {
        internal static void DisplayCharacterSheet(User user, Structure structure, Menu mainMenu)
        {
            string[] options = { "Details", "View Orders", "Logout" };
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
                        EditDetails.DisplayAndEditUserDetails(user, mainMenu, subMenu);
                    }
                    else if (subMenu.SelectedIndex == 1)
                    {
                        DisplayOrders(user, mainMenu, subMenu);
                    }
                    else if (subMenu.SelectedIndex == 2)
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

        private static void DisplayOrders(User user, Menu mainMenu, Menu subMenu)
        {
            while (true)
            {
                using (var dbContext = new MyDbContext())
                {
                    var orders = dbContext.Orders.Where(o => o.UserId == user.Id).ToList();
                    int selectedIndex = 0;

                    while (true)
                    {
                        Console.Clear();
                        Title.DisplayTitle();
                        mainMenu.DisplayOptions();
                        Console.WriteLine();
                        subMenu.DisplayOptions();
                        Console.WriteLine();
                        Console.WriteLine("Order History:");
                        Console.WriteLine("--------------");

                        for (int i = 0; i < orders.Count; i++)
                        {
                            if (i == selectedIndex)
                            {
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.BackgroundColor = ConsoleColor.White;
                            }
                            Console.WriteLine($"{i+1}. - Order ID: {orders[i].Id}, Date: {orders[i].Date:dd/MM/yyyy}, Total Price: {orders[i].TotalPrice} gold");
                            Console.ResetColor();
                        }

                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        ConsoleKey keyPressed = keyInfo.Key;

                        if (keyPressed == ConsoleKey.UpArrow)
                        {
                            selectedIndex--;
                            if (selectedIndex < 0)
                            {
                                selectedIndex = orders.Count - 1;
                            }
                        }
                        else if (keyPressed == ConsoleKey.DownArrow)
                        {
                            selectedIndex++;
                            if (selectedIndex >= orders.Count)
                            {
                                selectedIndex = 0;
                            }
                        }
                        else if (keyPressed == ConsoleKey.Enter)
                        {
                            DisplayOrderDetails(orders[selectedIndex], mainMenu, subMenu);
                        }
                        else if (keyPressed == ConsoleKey.Escape)
                        {
                            return;
                        }
                    }
                }
            }
        }

        private static void DisplayOrderDetails(Order order, Menu mainMenu, Menu subMenu)
        {
            while (true)
            {
                using (var dbContext = new MyDbContext())
                {
                    var orderProducts = dbContext.OrderProducts.Where(op => op.OrderId == order.Id).ToList();

                    Console.Clear();
                    Title.DisplayTitle();
                    mainMenu.DisplayOptions();
                    Console.WriteLine();
                    subMenu.DisplayOptions();
                    Console.WriteLine();
                    Console.WriteLine($"Order ID: {order.Id}");
                    Console.WriteLine($"Date: {order.Date:dd/MM/yyyy}");
                    Console.WriteLine($"Total Price: {order.TotalPrice} gold");
                    Console.WriteLine("Products:");
                    Console.WriteLine("---------");

                    foreach (var orderProduct in orderProducts)
                    {
                        var product = dbContext.Products.FirstOrDefault(p => p.Id == orderProduct.ProductId);
                        if (product != null)
                        {
                            Console.WriteLine($"Product: {product.Name}, Quantity: {orderProduct.Quantity}, Price: {orderProduct.Price} gold");
                        }
                    }
                    Console.WriteLine();
                    Console.WriteLine("Press Esc to return to the order history...");
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        return;
                    }
                }
            }
        }
    }
}