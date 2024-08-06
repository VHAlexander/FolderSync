namespace FolderSync
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: FolderSync <sourcePath> <destinationPath> <intervalInSeconds> <logFilePath>");
                return;
            }

            string sourcePath = args[0];
            string destinationPath = args[1];
            if (!double.TryParse(args[2], out double interval))
            {
                Console.WriteLine("Invalid interval. Please provide a valid number of seconds.");
                return;
            }
            string logPath = args[3];

            interval *= 1000; // Convert seconds to milliseconds

            /* -- Small class to create dummy files --
            
            FileManager fileManager = new FileManager();
            fileManager.CreateFile();

            */

            FolderSync folderSync = new FolderSync(sourcePath, destinationPath, logPath);
            folderSync.StartSync(interval);


            Console.WriteLine("Press [Enter] to stop the synchronization.");
            Console.ReadLine();
        }

        
    }
}
