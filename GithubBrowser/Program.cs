using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace GithubBrowser
{
    class Program {
        const int NumberOfApIs = 0;
        static readonly HttpClient Client = new HttpClient();

        public static string username;

        static async Task Main()
        {
            while(true)
            {
                GetUserInput();
                await GetGithubUserData();
                await GetGithubAPIAsync();
            }
        }

        static void GetUserInput() 
        {
            Console.WriteLine("Enter a valid github username: ");
            var userInput = Console.ReadLine();
            username = userInput;

            Console.WriteLine(" ");
            Console.WriteLine("---------------");
            Console.WriteLine("Selected username: " + username);
            Console.WriteLine("---------------");
            Console.WriteLine(" ");
            Console.WriteLine("Searching for user...");
            Console.WriteLine(" ");
        }

        static async Task GetGithubUserData()
        {
            var data = await GetUserDatas();

            Console.WriteLine(" ");
            Console.WriteLine("Username: " + data.Username);
            Console.WriteLine("Bio: " + data.Description);
            Console.WriteLine("Location: " + data.Location);
            Console.WriteLine("Followers: " + data.Followers);
            Console.WriteLine("Following: " + data.Following);
            Console.WriteLine(" ");
        }

        static async Task GetGithubAPIAsync()
        {
            var repos = await GetRepoDatas();
            Console.WriteLine($"{username} repositories: ");

            foreach(var repo in repos)
            {
                Console.WriteLine(" ");
                Console.WriteLine("Name: " + repo.Name);
                Console.WriteLine("Desc: " + repo.Description);
            }
        }

        static async Task<List<RepoData>> GetRepoDatas()
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            Client.DefaultRequestHeaders.Add("User-Agent", "my repo finder");

            var stream = Client.GetStreamAsync($"https://api.github.com/users/{username}/repos");
            var repos = await JsonSerializer.DeserializeAsync<List<RepoData>>(await stream);

            return repos;
        }

        static async Task<UserData> GetUserDatas()
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            Client.DefaultRequestHeaders.Add("User-Agent", "my repo finder");

            var stream = Client.GetStreamAsync($"https://api.github.com/users/{username}");
            var user = await JsonSerializer.DeserializeAsync<UserData>(await stream);

            return user;
        }
    }

    public class UserData
    {
        [JsonPropertyName("login")]
        public string Username {get; set;}

        [JsonPropertyName("bio")]
        public string Description {get; set;}

        [JsonPropertyName("location")]
        public string Location {get; set;}

        [JsonPropertyName("followers")]
        public int Followers {get; set;}

        [JsonPropertyName("following")]
        public int Following {get; set;}
    }

    public class RepoData
    {
        [JsonPropertyName("name")]
        public string Name {get; set;}

        [JsonPropertyName("description")]
        public string Description {get; set;}

        [JsonPropertyName("pushed_at")]
        public DateTime LastPushTime {get; set;}        
    }
}
