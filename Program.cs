using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Diagnostics;

namespace CYOA_Martel
{
    internal class Program
    {
        static string story; //stores the whole story as a string
        static string[] storyArray; //stores the story as pages
        static string[] currentPage; //stores the current page cut up into sections: text, page prompts, and destination numbers
        static int pageNum; //stores the current page

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
            if (FileExistsCheck("!story.txt")) //exits the application if there is no story file
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("ERROR: \"story.txt\" does not exist. Exiting application.");
                Console.ReadKey(true);
                throw new Exception();
            }

            return File.ReadAllText("story.txt");
        }
        static string[] SplitStory() //splits the story into pages
        {
            return story.Split('%');
        }
        
        static void WriteTitle() //writes the story title to the top of the screen
        {
            Console.SetCursorPosition(0, 0);

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            
            Console.WriteLine("--- " + storyArray[0] + " ---"); //writes the story title
        }

        static void WritePage(int page) //writes the given page on the screen
        {
            ErrorCheckPageNum(page);

            pageNum = page;

            Console.Clear();
            WriteTitle();

            Console.WriteLine();
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Page " + pageNum); //writes the current page numver on screen

            currentPage = storyArray[page].Split('$'); //assigns the current page to an array of page segments

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            foreach (string s in currentPage[0].Split('&')) //writes the text part of each page, adding a line break at each split
                Console.WriteLine(s);

            if (GameOverText())//checks if the current page is an ending page. if it is, prints game over message and quits game to menu
            {
                Restart();
            }
            ShowChoices();
            PickPage();
        }

        static void Restart() //returns the game to page 1
        {
            WritePage(1);
        }

        static void PickPage() //alows the reader to select which page to turn to, or allows them to enter a command
        {
            string input = Console.ReadLine();
            Console.WriteLine(input);

            if (input.ToLower() == "save")
            {
                Save();
                WritePage(pageNum);
            }
            else if (input.ToLower() == "load")
            {
                Load();
            }
            else if (input.ToLower() == "quit")
            {
                Restart();
            }
            else
            {
                foreach (string s in currentPage[2].Split('&')) //compares user input to potential page destinations
                {
                    if (input == s)
                    {
                        WritePage(Int32.Parse(s));
                    }
                }
                Console.Clear(); //the following runs if the user did not enter a valid option
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Not a valid option.");
                Console.ReadKey(true);
                WritePage(pageNum);
            }
        }

        static void ShowChoices() //writes the current page's options to the screen
        {
            if (ErrorCheckChoices())
            {
                Main();
                return;
            }

            for (int s = 0; s < currentPage[1].Split('&').Length; s++) //writes out all page destination choices
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
            Console.Write(" to return to page 1");
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Enter page number or command: ");
        }

        static bool GameOverText() //checks if current page is a game over page and writes a game over message if it is
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

        static void Save() //creates or overwrites savegame.txt and writes the current page number to it
        {
            File.WriteAllText("savegame.txt", pageNum.ToString());
        }
        static void Load() //calls WritePage with savegame.txt as the argument
        {
            try
            {
                WritePage(Int32.Parse(File.ReadAllText("savegame.txt")));
            }
            catch (Exception) //runs if savegame.txt does not contain a number, or if it does not exist
            {
                LoadGameError();
                WritePage(pageNum);
            }
            
        }

        static void LoadGameError() //tells the player that savegame.txt is missing or is not just a number
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("ERROR: Corrupt or missing save file.");
            Console.ReadKey(true);
        }

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

        static void ErrorCheckPageNum(int page) //checks if the destination page exists
        {
            try
            {
                string rangeCheck = storyArray[page];
            }
            catch (Exception)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("ERROR: Invalid page number.");
                Console.ReadKey(true);
                WritePage(pageNum);
            }
        }

        static bool FileExistsCheck(string fileCheck) //checks if the given file exists
        {
            return File.Exists(fileCheck);
        }
    }
}