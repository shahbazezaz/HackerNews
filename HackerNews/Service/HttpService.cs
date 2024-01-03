using HackerNews.Model;
using Newtonsoft.Json;

namespace HackerNews.Service
{
    public class HttpService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpService> _logger;

        public HttpService(IHttpClientFactory httpClientFactory, ILogger<HttpService> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
        }

        public async Task<List<int>> GetBestStoryIdsFromApiAsync()
        {
            try
            {
                var bestStoryIdsResponse = await _httpClient.GetAsync("https://hacker-news.firebaseio.com/v0/beststories.json");
                var bestStoryIds = await bestStoryIdsResponse.Content.ReadAsAsync<List<int>>();

                return bestStoryIds;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the best story IDs from the API.");
                throw;
            }
        }

        public async Task<Story> GetStoryByIdAsync(int storyId)
        {
            try
            {
                var storyResponse = await _httpClient.GetAsync($"https://hacker-news.firebaseio.com/v0/item/{storyId}.json");
                var storyJson = await storyResponse.Content.ReadAsStringAsync();
                var story = JsonConvert.DeserializeObject<Story>(storyJson);
                return story;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the story by ID.");
                throw;
            }
        }


    }

}
