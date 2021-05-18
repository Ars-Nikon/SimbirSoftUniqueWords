using System;
using System.Linq;

namespace SimbirSoftUniqueWords
{
    class Program
    {
        static void Main(string[] args)
        {
           
            HtmlUniqueWords test = new HtmlUniqueWords("https://www.simbirsoft.com/");
           
            test.SaveHtmlPage();

            foreach (var item in test.GetUniqueWords().OrderByDescending(x=>x.Value))
            {
                Console.WriteLine($"{item.Key} | {item.Value}");
            }
        }
    }

    class add : ILogger
    {
        public void Log(string logMessage)
        {
            throw new NotImplementedException();
        }
    }
}
