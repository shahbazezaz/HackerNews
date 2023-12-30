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
            var bestStories = await GetBestStoryIdsAsync(n);
            bestStories.Sort((s1, s2) => s2.Score.CompareTo(s1.Score));
            return bestStories;
        }

        private async Task<List<Story>> GetBestStoryIdsAsync(int n)
        {
            var cacheKey = "HackerNewsBestStoryIds";
            var bestStories = new List<Story>();
            if (!_cache.TryGetValue(cacheKey, out List<int> bestStoryIds))
            {
                bestStoryIds = await GetBestStoryIdsFromApiAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)); // Set cache expiration time

                _cache.Set(cacheKey, bestStoryIds, cacheOptions); // Cache the best story IDs
            }

            foreach (var storyId in bestStoryIds.Take(n))
            {
                var story = await GetStoryByIdAsync(storyId);
                bestStories.Add(story);
            }

            return bestStories;
        }

        private async Task<List<int>> GetBestStoryIdsFromApiAsync()
        {
            var bestStoryIdsResponse = await _httpClient.GetAsync("https://hacker-news.firebaseio.com/v0/beststories.json");
            return await bestStoryIdsResponse.Content.ReadAsAsync<List<int>>();
        }

        private async Task<Story> GetStoryByIdAsync(int storyId)
        {
            var bestStories = new List<Story>();
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
            return story;
        }
    }
}
