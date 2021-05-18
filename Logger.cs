using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SimbirSoftUniqueWords
{
    public interface ILogger
    {
        public void Log(string logMessage);
    }

    class Logger : ILogger
    {
        private string Path { get; set; }


        public Logger()
        {
            if (Path == null)
            {
                this.Path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\log.txt";
            }
            else
            {
                DirectoryInfo dirInfo = new DirectoryInfo(Path);
                if (!dirInfo.Exists)
                {
                    throw new Exception("Path not found");
                }
                if (Path[Path.Length - 1] != '\\')
                {
                    Path = Path + '\\';
                }
                this.Path = Path + @"\log.txt";
            }
        }

        public void Log(string logMessage)
        {
            StringBuilder log = new StringBuilder();
            log.Append(DateTime.Now + "|");
            log.Append(logMessage);

            using (StreamWriter sw = new StreamWriter(Path, true, System.Text.Encoding.Default))
            {
                sw.WriteLine(log.ToString());
            }
        }
    }
}
