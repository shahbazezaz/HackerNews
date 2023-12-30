using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNews.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BestStoriesController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public BestStoriesController(IHttpClientFactory httpClientFactory, IMemoryCache cache)
        {
            _httpClient = httpClientFactory.CreateClient();
            _cache = cache;
        }

        [HttpGet("{n}")]
        public async Task<ActionResult<List<Story>>> GetBestStories(int n)
        {
            var bestStoryIds = await GetBestStoryIdsAsync();
            var bestStories = await GetBestStoriesAsync(bestStoryIds, n);
            bestStories.Sort((s1, s2) => s2.Score.CompareTo(s1.Score));
            return bestStories;
        }

        private async Task<List<int>> GetBestStoryIdsAsync()
        {
            var bestStoryIdsResponse = await _httpClient.GetAsync("https://hacker-news.firebaseio.com/v0/beststories.json");
            return await bestStoryIdsResponse.Content.ReadAsAsync<List<int>>();
        }

        private async Task<List<Story>> GetBestStoriesAsync(List<int> bestStoryIds, int n)
        {
            var bestStories = new List<Story>();

            foreach (var storyId in bestStoryIds.Take(n))
            {
                var cacheKey = $"Story_{storyId}";
                if (!_cache.TryGetValue(cacheKey, out Story story))
                {
                    var storyResponse = await _httpClient.GetAsync($"https://hacker-news.firebaseio.com/v0/item/{storyId}.json");
                    var storyJson = await storyResponse.Content.ReadAsStringAsync();
                    story = JsonConvert.DeserializeObject<Story>(storyJson);

                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)); // Set cache expiration time

                    _cache.Set(cacheKey, story, cacheOptions); // Cache the story
                }

                bestStories.Add(story);
            }
            return bestStories;
        }
    }
}
