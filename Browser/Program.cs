using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Browser
{
    static class Program
    {
        private const string Host = "www.acme.com";
        private static string _path = "/";
        
        private const int Port = 80;

        private static TcpClient _client;
        private static NetworkStream _stream;

        private static readonly List<Page> Pages = new List<Page>();
        
        private static void Main()
        {
            while (true)
            {
                _client = new TcpClient(Host, Port);
                _stream = _client.GetStream();
                
                var data = "GET " + _path + " HTTP/1.1\r\n" + "Host:" + Host + "\r\n" + "\r\n";
                _stream.Write(Encoding.UTF8.GetBytes(data));
                
                var reader = new StreamReader(_stream);
                var html = reader.ReadToEnd();
                
                GetPages(html);
                Console.WriteLine(Pages.Count);
                Console.WriteLine($"Loading Page...");
                Console.WriteLine("<----- Loaded Page ----->");
                Console.WriteLine("Page: " + GetPageTitle(html));
                
                DisplayLinks();

                GetUserInput();
                
                _stream.Close();
                _client.Close();
            }
        }

        private static void GetUserInput()
        {
            Console.WriteLine("What link do you want to go to?");
            var input = Console.ReadLine();
            var pageNum = int.Parse(input ?? string.Empty);

            if (pageNum > Pages.Count)
            {
                Console.WriteLine("The page you want to browse does not exist.");
                return;
            }
            else
            {
                Console.WriteLine($"Going to {pageNum}: {Pages[pageNum].Link}");
                
                _path = Pages[pageNum].Link;
                _path = _path.TrimStart('/');
                _path = "/" + _path;
            }
        }
        
        private static void DisplayLinks()
        {
            foreach (var page in Pages)
            {
                Console.Write($"<{Pages.IndexOf(page)}>");
                Console.WriteLine($" [{page.Link}]");
            }
        }
        
        private static void GetPages(string page)
        {
            Pages.Clear();
            var regex = new Regex("<a href=[\"|'](?<link>.*?)[\"|'].*?>(<b>|<img.*?>)?(?<name>.*?)(</b>)?</a>", RegexOptions.None);
            
            if (!regex.IsMatch(page)) return;
            
            foreach (Match match in regex.Matches(page))
            {
                Pages.Add(new Page(match.Groups["link"].Value, match.Groups["title"].Value));
            }
        }

        private static string GetPageTitle(string page)
        {
            var m = Regex.Match(page, @"<title>\s*(.+?)\s*</title>");
            return m.Success ? m.Groups[1].Value : "";
        }

        private struct Page
        {
            public string Link;
            public string Title;

            public Page(string link, string title)
            {
                this.Link = link;
                this.Title = title;
            }
        }
    }
}