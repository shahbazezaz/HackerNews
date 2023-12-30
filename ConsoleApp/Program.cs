using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        using (HttpClient client = new HttpClient())
        {
            string apiUrl = "https://localhost:8080/BestStories/10"; // Replace with the actual API URL

            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine($"API request failed with status code: {response.StatusCode}");
            }
        }

    }
}
