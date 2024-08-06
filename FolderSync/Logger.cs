using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderSync
{
    public class Logger
    {
        private string logFilePath;

        public Logger(string logFilePath)
        {
            this.logFilePath = logFilePath;
        }

        public void Log(string logMessage)
        {
            string logDir = Path.GetDirectoryName(logFilePath);
            DirectoryInfo logDirectory = new DirectoryInfo(logDir);

            if (!logDirectory.Exists )
            {
                logDirectory.Create();
                Console.WriteLine("Creating specified directory..");
                logFilePath = logDir + "\\" + Path.GetFileName(logFilePath);
            }

            
            using (StreamWriter w = File.AppendText(logFilePath))
            {
                WriteLog(logMessage, w);
            }
        }

        private void WriteLog(string logMessage, TextWriter w)
        {
            
            w.Write($"\n{DateTime.Now.ToLongTimeString()} , {DateTime.Now.ToLongDateString()}");
            w.Write($": {logMessage}");
        }
        
        /* - unused code for dumping everything in the log file to the console -
        
        public void DumpLog()
        {
            using (StreamReader r = File.OpenText(logFilePath))
            {
                ReadLog(r);
            }
        }

        private void ReadLog(StreamReader r)
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }

        */
    }
}
