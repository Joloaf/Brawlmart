namespace BrawlmartTest
{
    public static class Title
    {
        public static void DisplayTitle()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
               _________.                      .__      _____                 __.  
               \______   \_______.____ __  _  _|  |    /     \ .____ ________/  |_ 
                |   |/  _/\_  __ \__. \\ \/ \/ /  |   /  . .  \\__. \\_  __ \   __\
                |   |\   \ |  | \// __ \\     /|  |__/   |_|   \/ __ \|  | \/|  |  
                |______  / |__|  (____  /\/\_/ |____/\__/   \  (____  /__|   |__|  
                       \/             \/                     \/     \/             
        ");
            Console.ResetColor();
        }
    }
}