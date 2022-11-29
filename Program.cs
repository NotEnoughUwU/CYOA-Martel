using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CYOA_Martel
{
    internal class Program
    {
        static string story; //stores the whole story as a string
        static string[] storyArray; //stores the story as pages
        static string[] currentPage; //stores the current page cut up into sections: text, page prompts, and destination numbers

        static void Main()
        {
            Console.SetCursorPosition(0, 0); //resets cursor position if this isn't the first time calling Main
            story = GetStory();
            storyArray = SplitStory();
            WritePage(1);
            Console.ReadKey(true);
        }

        static string GetStory() //puts the story into a string from a text file
        {
            return File.ReadAllText("Story.txt");
        }
        static string[] SplitStory() //splits the story into pages
        {
            return story.Split('%');
        }
        
        static void WriteTitle()
        {
            Console.SetCursorPosition(0, 0);

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            
            Console.WriteLine("--- " + storyArray[0] + " ---"); //writes the story title
        }

        static void WritePage(int page)
        {
            Console.Clear();
            WriteTitle();

            currentPage = storyArray[page].Split('$');

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            foreach (string s in currentPage[0].Split('&')) //writes the text part of each page, adding a line break at each split
                Console.WriteLine(s);

            if (GameOverText())//checks if the current page is an ending page. if it is, prints game over message and quits game to menu
            {
                Main();
                return;
            }
            ShowChoices();
            PickPage();
        }

        static void PickPage()
        {
            string input = Console.ReadLine();

            if (input.ToLower() == "save")
            {
                //Save();
            }
            else if (input.ToLower() == "load")
            {
                //Load();
            }
            else if (input.ToLower() == "quit")
            {
                //Quit();
            }
            else
            {
                foreach (string s in currentPage[2].Split('&'))
                {
                    if (input == s)
                    {
                        WritePage(Int32.Parse(s));
                    }
                }
            }
        }

        static void ShowChoices()
        {
            if (ErrorCheckChoices())
            {
                Main();
                return;
            }

            for (int s = 0; s < currentPage[1].Split('&').Length; s++)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Turn to page " + currentPage[2].Split('&')[s] + ": ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(currentPage[1].Split('&')[s]);
            }

            Console.WriteLine();
            Console.Write("Type ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\"save\"");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" to save current game");
            Console.WriteLine();
            Console.Write("Type ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\"load\"");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" to load saved game");
            Console.WriteLine();
            Console.Write("Type ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("\"quit\"");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" to return to menu");
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Enter page number or command: ");
        }

        static bool GameOverText()
        {
            if (currentPage[1][0] == '!') //if reading game over text
            {
                for (int i = 0; i < currentPage[1].Length; i++) //writes "-" for the length of the game-over text
                    Console.Write('-');
                Console.WriteLine();

                Console.WriteLine(currentPage[1].Remove(0, 1));

                for (int i = 0; i < currentPage[1].Length; i++)
                    Console.Write('-');

                Console.ReadKey(true);
                return true;
            }
            return false;
        }

        //Error checking methods ---------------------------------------------------------------------------------

        static bool ErrorCheckChoices() //checks if the amount of destination numbers and page prompts are equal
        {
            if (currentPage[1].Split('&').Length != currentPage[2].Split('&').Length)
            {
                Console.Clear();
                Console.SetCursorPosition(0,0);
                Console.WriteLine("ERROR: Invalid choices. The amount of destination choices do not match with the amount of page prompts.");
                Console.ReadKey(true);
                return true;
            }
            return false;
        }
    }
}