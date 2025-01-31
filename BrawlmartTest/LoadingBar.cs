namespace BrawlmartTest
{
    internal class LoadingBar
    {
        public static void DisplayLoadingBar()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(22, 10);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Loading Brawlville Marketplace...");
            Random rnd = new Random();
            Console.SetCursorPosition(21, 11);
            Console.Write("[");
            Console.SetCursorPosition(72, 11);
            Console.Write("]");
            for (int i = 0; i < 51; i++)
            {
                for (int y = 0; y < i; y++)
                {
                    string pb = "\u2551";
                    Console.Write(pb);
                }
                Console.SetCursorPosition(22, 11);
                
                System.Threading.Thread.Sleep(rnd.Next(1, 200));
            }
            
            Console.ResetColor();
        }
    }
}