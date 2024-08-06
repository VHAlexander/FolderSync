using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Timers;



namespace FolderSync
{
    public class FolderSync
    {
        private string sourcePath;
        private string destinationPath;
        private string logFilePath;
        private static System.Timers.Timer syncTimer;
        private readonly Logger logger;
        private HashSet<string> knownSourceFiles;
        private List<string> latestChanges;

        public FolderSync(string sourcePath, string destinationPath, string logFilePath)
        {
            this.sourcePath = sourcePath;
            this.destinationPath = destinationPath;
            this.logFilePath = logFilePath;
            this.logger = new Logger(logFilePath);
            this.knownSourceFiles = new HashSet<string>(Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories));
            this.latestChanges = new List<string>();
        }


        public void StartSync(double interval)
        {
            if (!Directory.Exists(sourcePath))
            {
                Console.WriteLine($"Source path does not exist: {sourcePath}");
                return;
            }

            SetTimer(interval);
            Console.WriteLine("Folder synchronization started.");
            
        }

        private void SyncDirectories(string sourceDir, string destDir)
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(sourceDir);
            DirectoryInfo destinationDirectory = new DirectoryInfo(destDir);
                      

            if (!destinationDirectory.Exists)
            {
                destinationDirectory.Create();
                LogChange($"Created directory: {destinationDirectory.FullName}");
            }

            foreach (FileInfo file in sourceDirectory.GetFiles())
            {
                if (knownSourceFiles.Add(file.FullName))
                {
                    LogChange($"Added file: {file.FullName}");
                }
            }

            foreach (FileInfo file in sourceDirectory.GetFiles())
            {
                string destinationFilePath = Path.Combine(destinationDirectory.FullName, file.Name);
                
                if (!File.Exists(destinationFilePath) || file.LastWriteTime > File.GetLastWriteTime(destinationFilePath))
                {
                    file.CopyTo(destinationFilePath, true);
                    LogChange($"Copied file: {file.FullName} to {destinationFilePath}");
                }
            }

            foreach (DirectoryInfo subDir in sourceDirectory.GetDirectories())
            {
                string destinationSubDirPath = Path.Combine(destinationDirectory.FullName, subDir.Name);
                SyncDirectories(subDir.FullName, destinationSubDirPath);
            }

            foreach (FileInfo file in destinationDirectory.GetFiles())
            {
                string sourceFilePath = Path.Combine(sourceDirectory.FullName, file.Name);
                if (!File.Exists(sourceFilePath))
                {
                    file.Delete();
                    LogChange($"Deleted file: {file.FullName}");
                }
            }

            foreach (DirectoryInfo subDir in destinationDirectory.GetDirectories())
            {
                string sourceSubDirPath = Path.Combine(sourceDirectory.FullName, subDir.Name);
                if (!Directory.Exists(sourceSubDirPath))
                {
                    subDir.Delete(true);
                    LogChange($"Deleted directory: {subDir.FullName}");
                }
            }

        }



        private void SetTimer(double interval)
        {
            // Create a timer with a user defined interval.
            syncTimer = new System.Timers.Timer(interval);
            // Hook up the Elapsed event for the timer. 
            syncTimer.Elapsed += OnTimedEvent;
            syncTimer.AutoReset = true;
            syncTimer.Enabled = true;
        }


        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Synchronization successful at {0:HH:mm:ss}",
                              e.SignalTime);
            try
            {
                SyncDirectories(sourcePath, destinationPath);
                Console.WriteLine("Folders synchronized successfully.");


                // Prevents enumeration error from occurring by saving changes to a a different list.
                List<string> changesToDisplay;
                lock (latestChanges)
                {
                    changesToDisplay = new List<string>(latestChanges);
                    latestChanges.Clear();
                }

                if (changesToDisplay.Count > 0)
                {
                    Console.WriteLine("Latest modifications:");
                    foreach (var change in changesToDisplay)
                    {
                        Console.WriteLine(change);
                    }
                }
                else
                {
                    Console.WriteLine("No new modifications.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            
            
        }

        private void LogChange(string message)
        {
            logger.Log(message);
            latestChanges.Add(message);
        }
    }
}
