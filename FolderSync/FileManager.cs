using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderSync
{
    public class FileManager
    {
        public string fileName {  get; set; }
        public FileManager()
        {

        }
        public void CreateFile()
        {
            ConsoleKeyInfo cki;

            Console.WriteLine("Create dummy files? Press 'y' to continue \n");
            cki = Console.ReadKey(true);

            if (cki.Key == ConsoleKey.Y)
            {
                do
                {

                    do
                    {
                        Console.Write("Please enter a filename: ");
                        fileName = Console.ReadLine();
                    }
                    while (string.IsNullOrWhiteSpace(fileName)); // Validate input

                    Console.WriteLine($"You entered: {fileName}");


                    string path = @"c:\SourceFolder\" + fileName + ".txt";
                    if (!File.Exists(path))
                    {
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path))
                        {

                            sw.WriteLine("Hello");
                            sw.WriteLine("And");
                            sw.WriteLine("Welcome");
                        }
                    }

                    Console.WriteLine("Press the any key to continue or Escape (Esc) key to quit: \n");
                    cki = Console.ReadKey(intercept: true); //without intercept:true, first character in next line disappears

                } while (cki.Key != ConsoleKey.Escape);
            }
            
        }
    }
}
