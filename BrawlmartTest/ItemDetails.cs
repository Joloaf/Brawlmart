using System.Text;
using BrawlmartTest.Models;

namespace BrawlmartTest
{
    internal class ItemDetails
    {
        public static void DisplayItemDetails(Product product, Menu mainMenu, Menu marketMenu)
        {
            using (var dbContext = new MyDbContext())
            {
                product.Category = dbContext.Categories.FirstOrDefault(c => c.Id == product.CategoryId);
            }

            ConsoleKey keyPressed;

            do
            {
                Console.Clear();
                Title.DisplayTitle();
                mainMenu.DisplayOptions();
                Console.WriteLine();

                if (marketMenu != null)
                {
                    marketMenu.DisplayOptions();
                }

                Console.WriteLine();
                Console.WriteLine("Name: {0,-25} Category: {1}", product.Name, product.Category?.Name ?? "Unknown");
                Console.WriteLine(new string('-', 100));
                Console.WriteLine("{0,-15} {1,-15} {2,-15} {3,-20} {4,-10} {5,20}", "Level", "Rarity", "Color", "Material", "Size", "Price");
                Console.WriteLine(new string('=', 100));
                if (product.Stock == 0)
                {
                    Console.WriteLine("{0,-15} {1,-15} {2,-15} {3,-20} {4,-10} {5,20}", product.Level, product.Rarity, product.Color, product.Material, product.Size, "   Out of stock");
                }
                else
                {
                    Console.WriteLine("{0,-15} {1,-15} {2,-15} {3,-20} {4,-15} {5,10} gold", product.Level, product.Rarity, product.Color, product.Material, product.Size, product.Price);
                }
                Console.WriteLine();
                Console.WriteLine("Details:");
                Console.WriteLine(WrapText(product.Details ?? string.Empty, 100));
                Console.WriteLine(new string('-', 100));
                Console.WriteLine();

                DisplayAverageRatingAndFeedback(product);

                if (Login.IsLoggedIn())
                {
                    if (product.Stock == 0)
                    {
                        Console.WriteLine("Press R to rate, F to leave feedback, or any other key to return...");
                    }
                    else
                    {
                        Console.WriteLine("Press B to add to cart, R to rate, F to leave feedback, or any other key to return...");
                    }
                    Console.WriteLine();
                }
                else
                {
                    if (product.Stock == 0)
                    {
                        Console.WriteLine("Press any other key to return...");
                    }
                    else
                    {
                        Console.WriteLine("Press B to add to cart, or any other key to return...");
                    }
                    Console.WriteLine();
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.B && product.Stock > 0)
                {
                    Cart.AddToCart(product);
                    Console.WriteLine("Item added to cart.");
                    System.Threading.Thread.Sleep(1000);
                }
                else if (keyPressed == ConsoleKey.R && Login.IsLoggedIn())
                {
                    RateProduct(product);
                }
                else if (keyPressed == ConsoleKey.F && Login.IsLoggedIn())
                {
                    LeaveFeedback(product);
                }
                else
                {
                    return;
                }
            } while (true);
        }

        private static void DisplayAverageRatingAndFeedback(Product product)
        {
            using (var dbContext = new MyDbContext())
            {
                var feedbacks = dbContext.Feedbacks
                    .Where(f => f.ProductId == product.Id && !string.IsNullOrEmpty(f.UserNote))
                    .Select(f => new
                    {
                        f.UserRating,
                        f.UserNote,
                        UserName = dbContext.Users.FirstOrDefault(u => u.Id == f.UserId).UserName
                    })
                    .ToList();

                var averageRating = dbContext.Feedbacks
                    .Where(f => f.ProductId == product.Id)
                    .Average(f => (double?)f.UserRating) ?? 0;

                string averageRatingDisplay = averageRating % 1 == 0 ? averageRating.ToString("F0") : averageRating.ToString("F1");

                Console.WriteLine($"Rating: {averageRatingDisplay} / 5");
                Console.WriteLine();
                Console.WriteLine("Feedback:");
                Console.WriteLine();
                foreach (var feedback in feedbacks)
                {
                    if (feedback.UserRating.HasValue)
                    {
                        Console.WriteLine($"{feedback.UserName}: {WrapText(feedback.UserNote, 90)} (Rating: {feedback.UserRating}/5)");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine($"{feedback.UserName}: {WrapText(feedback.UserNote, 90)}");
                        Console.WriteLine();
                    }
                }
                Console.WriteLine();
            }
        }

        private static void RateProduct(Product product)
        {
            Console.WriteLine("Enter your rating (1-5) or press Escape to cancel: ");
            StringBuilder input = new StringBuilder();
            ConsoleKeyInfo keyInfo;

            while (true)
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("\nRating canceled.");
                    return;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (input.Length > 0)
                    {
                        input.Remove(input.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                }
                else if (char.IsDigit(keyInfo.KeyChar) && input.Length < 1)
                {
                    input.Append(keyInfo.KeyChar);
                    Console.Write(keyInfo.KeyChar);
                }
            }

            if (int.TryParse(input.ToString(), out int rating) && rating >= 1 && rating <= 5)
            {
                using (var dbContext = new MyDbContext())
                {
                    var feedback = dbContext.Feedbacks.FirstOrDefault(f => f.ProductId == product.Id && f.UserId == Login.GetCurrentUser().Id);
                    if (feedback == null)
                    {
                        feedback = new Feedback { ProductId = product.Id, UserId = Login.GetCurrentUser().Id, UserRating = rating };
                        dbContext.Feedbacks.Add(feedback);
                    }
                    else
                    {
                        feedback.UserRating = rating;
                    }
                    dbContext.SaveChanges();
                }
                Console.WriteLine("Rating saved.");
                System.Threading.Thread.Sleep(1000);
            }
            else
            {
                Console.WriteLine("Invalid rating. Please enter a number between 1 and 5.");
                System.Threading.Thread.Sleep(1000);
            }
        }

        private static void LeaveFeedback(Product product)
        {
            Console.WriteLine("Enter your feedback or press Escape to cancel: ");
            StringBuilder feedbackNote = new StringBuilder();
            ConsoleKeyInfo keyInfo;
            int maxLineLength = 100;
            int currentLineLength = 0;

            while (true)
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("\nFeedback canceled.");
                    return;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (feedbackNote.Length > 0)
                    {
                        feedbackNote.Remove(feedbackNote.Length - 1, 1);
                        if (currentLineLength > 0)
                        {
                            currentLineLength--;
                        }
                        else
                        {
                            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                            currentLineLength = maxLineLength - 1;
                        }
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    feedbackNote.Append(keyInfo.KeyChar);
                    Console.Write(keyInfo.KeyChar);
                    currentLineLength++;
                    if (currentLineLength >= maxLineLength)
                    {
                        Console.WriteLine();
                        currentLineLength = 0;
                    }
                }
            }

            using (var dbContext = new MyDbContext())
            {
                var feedback = dbContext.Feedbacks.FirstOrDefault(f => f.ProductId == product.Id && f.UserId == Login.GetCurrentUser().Id);
                if (feedback == null)
                {
                    feedback = new Feedback { ProductId = product.Id, UserId = Login.GetCurrentUser().Id, UserNote = feedbackNote.ToString() };
                    dbContext.Feedbacks.Add(feedback);
                }
                else
                {
                    feedback.UserNote = feedbackNote.ToString();
                }
                dbContext.SaveChanges();
            }
            Console.WriteLine("Feedback saved.");
            System.Threading.Thread.Sleep(1000);
        }

        private static string WrapText(string text, int maxLineLength)
        {
            var words = text.Split(' ');
            var wrappedText = new StringBuilder();
            var currentLineLength = 0;

            foreach (var word in words)
            {
                if (currentLineLength + word.Length + 1 > maxLineLength)
                {
                    wrappedText.AppendLine();
                    currentLineLength = 0;
                }

                if (currentLineLength > 0)
                {
                    wrappedText.Append(' ');
                    currentLineLength++;
                }

                wrappedText.Append(word);
                currentLineLength += word.Length;
            }

            return wrappedText.ToString();
        }
    }
}