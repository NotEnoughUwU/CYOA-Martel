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
        static string story;
        static string[] storyArray;
        static string[] currentPage;

        static void Main(string[] args)
        {
            story = GetStory();
            storyArray = SplitStory();
            WritePage(8);
            Console.ReadKey(true);
        }

        static string GetStory()
        {
            return System.IO.File.ReadAllText("Story.txt");
        }
        static string[] SplitStory()
        {
            return story.Split('%');
        }
        
        static void WriteTitle()
        {
            Console.SetCursorPosition(0, 0);

            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.ForegroundColor = ConsoleColor.White;
            
            Console.WriteLine("--- " + storyArray[0] + " ---"); //writes the story title
        }

        static void WritePage(int page)
        {
            Console.Clear();
            WriteTitle();

            currentPage = storyArray[page].Split('$');

            Console.SetCursorPosition(0, 1);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            foreach (string s in currentPage[0].Split('&'))//writes the text part of each page, adding a line break at each split
                Console.WriteLine(s);

            if (currentPage[1][0] == '!')//if (reading game over text)
            {
                for (int i = 0; i < currentPage[1].Length - 3; i++)//writes "-" for the length of the game over text minus 1
                    Console.Write('-');
                Console.WriteLine();

                Console.Write(currentPage[1].Remove(0, 1));

                for (int i = 0; i < currentPage[1].Length - 3; i++)
                    Console.Write('-');
            }
        }
    }
}