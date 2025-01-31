using BrawlmartTest.Models;

namespace BrawlmartTest
{
    internal class OrderHistory
    {
        internal static void DisplayOrderDetails(Order order, Menu mainMenu, Menu subMenu)
        {
            using (var dbContext = new MyDbContext())
            {
                var orderDetails = dbContext.Orders
                    .Where(o => o.Id == order.Id)
                    .Select(o => new
                    {
                        o.Id,
                        o.Date,
                        o.TotalPrice,
                        User = o.User,
                        OrderProducts = o.OrderProducts.Select(op => new
                        {
                            op.Product.Name,
                            op.Quantity,
                            op.Price
                        }).ToList()
                    }).FirstOrDefault();

                if (orderDetails == null)
                {
                    Console.WriteLine("Order not found.");
                    Console.WriteLine("Press any key to return...");
                    Console.ReadKey(true);
                    return;
                }

                while (true)
                {
                    Console.Clear();
                    Title.DisplayTitle();
                    mainMenu.DisplayOptions();
                    Console.WriteLine();
                    subMenu.DisplayOptions();
                    Console.WriteLine();
                    Console.WriteLine("Order Details");
                    Console.WriteLine("-------------");
                    Console.WriteLine($"Order ID: {orderDetails.Id}");
                    Console.WriteLine($"User Name: {orderDetails.User.UserName}");
                    Console.WriteLine($"Date: {orderDetails.Date:dd/MM/yyyy}");
                    Console.WriteLine($"Total Price: {orderDetails.TotalPrice} gold");
                    Console.WriteLine();
                    Console.WriteLine("User Details");
                    Console.WriteLine("------------");
                    Console.WriteLine($"First Name: {orderDetails.User.FirstName}");
                    Console.WriteLine($"Last Name: {orderDetails.User.LastName}");
                    Console.WriteLine($"Address: {orderDetails.User.StreetAddress}, {orderDetails.User.PostalCode}, {orderDetails.User.City}, {orderDetails.User.Country}");
                    Console.WriteLine($"Email: {orderDetails.User.Email}");
                    Console.WriteLine($"Phone Number: {orderDetails.User.PhoneNumber}");
                    Console.WriteLine();
                    Console.WriteLine("Ordered Items");
                    Console.WriteLine("-------------");
                    Console.WriteLine("{0,-40} {1,-10} {2,-10}", "Item Name", "Quantity", "Price");
                    Console.WriteLine(new string('-', 60));

                    foreach (var item in orderDetails.OrderProducts)
                    {
                        Console.WriteLine("{0,-40} {1,-10} {2,-10} gold", item.Name, item.Quantity, item.Price);
                    }
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