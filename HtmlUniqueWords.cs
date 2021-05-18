using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace SimbirSoftUniqueWords
{
    public class HtmlUniqueWords
    {
        private ILogger Logger { get; set; }
        public string Url { get; set; }
        private string Path { get; set; } = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\";
        public Func<string, bool> FilterWords { get; set; } = x => Regex.IsMatch(x, @"\w") && Regex.IsMatch(x, @"\D");
        public char[] SplitFilter { get; set; } = new char[] { ' ', ',', '.', '!', '?', '"', ';', ':', '[', ']', '(', ')', '\n', '\r', '\t' };
        public string NameFile { get; set; }
        private WebClient client { get; set; }
        public string UserAgent { get; set; } = "User-Agent: Chrome";

        public HtmlUniqueWords()
        {
            client = new WebClient();
            client.Headers.Add(UserAgent);
            Logger = new Logger();
        }
        public HtmlUniqueWords(string Url)
        {
            if (Url != null)
            {
                this.Url = Url;
            }
            client = new WebClient();
            client.Headers.Add(UserAgent);
            Logger = new Logger();
        }
        public HtmlUniqueWords(string Url, string Path)
        {
            if (Url != null)
            {
                this.Url = Url;
            }
            if (Path != null)
            {
                this.Path = Path;
            }
            Logger = new Logger();
            client = new WebClient();
            client.Headers.Add(UserAgent);
        }
        public HtmlUniqueWords(ILogger logger, string Url = null, string Path = null)
        {
            if (Url != null)
            {
                this.Url = Url;
            }
            if (Path != null)
            {
                this.Path = Path;
            }
            this.Logger = logger;
            client = new WebClient();
            client.Headers.Add(UserAgent);
        }

        public void SaveHtmlPage()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path);
            if (!dirInfo.Exists)
            {
                Logger.Log("Path not found");
                throw new Exception("Path not found");
            }
            if (Path[Path.Length - 1] != '\\')
            {
                Path = Path + '\\';
            }
            if (Url == null)
            {
                Logger.Log("Url NullReferenceException");
                throw new Exception("Url NullReferenceException");
            }
            else
            {
                try
                {
                    using (Stream stream = client.OpenRead(Url))
                    {

                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message);
                    throw;
                }

            }
            if (NameFile == null)
            {
                int a = Url.Length;
                string Name = Url.Substring(Url.IndexOf(" / /") + 2);
                if (Name.IndexOf(" /") > 0)
                {
                    Name = Name.Substring(0, Name.IndexOf("/"));
                    Name = Name.Substring(0, Name.LastIndexOf("."));
                }
                else
                {
                    Name = Name.Substring(0, Name.LastIndexOf("."));
                }
                if (Name.IndexOf('.') > 0)
                {
                    Name = Name.Substring(Name.IndexOf("."));
                }
                Name = Name.Replace('.', ' ');

                if (Name.IndexOf('\\') > 0 || Name.IndexOf("/") > 0 || Name.IndexOf(":") > 0 || Name.IndexOf("*") > 0 || Name.IndexOf("\"") > 0 || Name.IndexOf("<") > 0 || Name.IndexOf(">") > 0 || Name.IndexOf("|") > 0)
                {
                    Logger.Log("The filename should not contain these characters \\/:*?\"<>|");
                    throw new Exception("The filename should not contain these characters \\/:*?\"<>|");
                }
                Path = Path + Name + ".html";
            }
            else
            {
                if (NameFile.IndexOf('\\') > 0 || NameFile.IndexOf("/") > 0 || NameFile.IndexOf(":") > 0 || NameFile.IndexOf("*") > 0 || NameFile.IndexOf("\"") > 0 || NameFile.IndexOf("<") > 0 || NameFile.IndexOf(">") > 0 || NameFile.IndexOf("|") > 0)
                {
                    Logger.Log("The filename should not contain these characters \\/:*?\"<>|");
                    throw new Exception($"The filename should not contain these characters \\/:*?\"<>|");
                }
                Path = Path + NameFile + ".html";
            }
            client.DownloadFile(Url, Path);
        }

        public Dictionary<string, int> GetUniqueWords()
        {
            if (Url == null)
            {
                Logger.Log("Url NullReferenceException");
                throw new Exception("Url NullReferenceException");
            }

            string stringHtmlPage;
            try
            {
                stringHtmlPage = client.DownloadString(Url);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                throw;
            }
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(stringHtmlPage);
            StringBuilder strbuildInnetText = new StringBuilder();

            foreach (var Node in htmlDoc.DocumentNode.SelectSingleNode("//body").ChildNodes)
            {
                if (Node.XPath.IndexOf("script") > 0)
                {
                    continue;
                }
                strbuildInnetText.Append(Node.InnerText);
            }

            string htmlText = strbuildInnetText.ToString().ToLower();
            var splitText = htmlText.Split(SplitFilter);

            var words = splitText.Where(FilterWords).ToArray();

            Dictionary<string, int> uniqueWords = new Dictionary<string, int>();

            foreach (var word in words)
            {
                if (uniqueWords.ContainsKey(word))
                {
                    uniqueWords[word] = uniqueWords[word] + 1;
                }
                else
                {
                    uniqueWords.Add(word, 1);
                }
            }
            return uniqueWords;

        }
    }
}
