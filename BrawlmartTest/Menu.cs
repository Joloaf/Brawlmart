using static System.Console;

namespace BrawlmartTest
{
    public class Menu
    {
        public int SelectedIndex { get; set; }
        public string[] Options;
        private const int ButtonWidth = 20;
        public bool DisableArrowKeys { get; set; }

        public Menu(string[] options)
        {
            Options = options;
            //SelectedIndex = 0;
        }

        public void DisplayOptions()
        {
            for (int i = 0; i < Options.Length; i++)
            {
                string currentOption = Options[i];
                if (i == SelectedIndex)
                {
                    ForegroundColor = ConsoleColor.Black;
                    BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    ForegroundColor = ConsoleColor.White;
                    BackgroundColor = ConsoleColor.Black;
                }

                int totalPadding = ButtonWidth - 2 - currentOption.Length;
                int leftPadding = totalPadding / 2;
                int rightPadding = totalPadding - leftPadding;
                string paddedOption = $"[{new string(' ', leftPadding)}{currentOption}{new string(' ', rightPadding)}]";
                paddedOption = paddedOption.PadRight(ButtonWidth);
                Write(paddedOption);
                ResetColor();
            }
            WriteLine();
        }

        public void UpdateOption(int index, string newOption)
        {
            if (index >= 0 && index < Options.Length)
            {
                Options[index] = newOption;
            }
        }

        // Ended up not using this method but will probably move back to using it in the future...
        // Hope to get more modular navigation code in the next iteration... 2.0, here we come!
        public int Run()
        {
            ConsoleKey keyPressed;
            do
            {
                Clear();
                Title.DisplayTitle();
                DisplayOptions();

                ConsoleKeyInfo keyInfo = ReadKey(true);
                keyPressed = keyInfo.Key;
                if (!DisableArrowKeys)
                {
                    if (keyPressed == ConsoleKey.LeftArrow)
                    {
                        SelectedIndex--;
                        if (SelectedIndex == -1)
                        {
                            SelectedIndex = Options.Length - 1;
                        }
                    }
                    else if (keyPressed == ConsoleKey.RightArrow)
                    {
                        SelectedIndex++;
                        if (SelectedIndex == Options.Length)
                        {
                            SelectedIndex = 0;
                        }
                    }
                }
            } while (keyPressed != ConsoleKey.Enter);

            return SelectedIndex;
        }
    }
}