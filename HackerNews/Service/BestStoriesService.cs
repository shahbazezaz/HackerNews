using HackerNews.Model;

namespace HackerNews.Service
{
    public class BestStoriesService
    {
        private readonly HttpService _httpService;
        private readonly LoggerService _loggerService;
        private readonly CacheService _cacheService;

        public BestStoriesService(HttpService httpService, LoggerService loggerService, CacheService cacheService)
        {
            _httpService = httpService;
            _loggerService = loggerService;
            _cacheService = cacheService;
        }

        public async Task<ResponseModel> GetBestStoryIdsAsync(RequestModel requestModel)
        {
            var cacheKey = "HackerNewsBestStoryIds";
            var responseModel = new ResponseModel();

            // Fetch the best story IDs from the cache or API
            var bestStoryIds = await GetBestStoryIdsFromCacheOrApiAsync(cacheKey);

            // Fetch the stories based on the request parameters
            var bestStories = await FetchStoriesAsync(bestStoryIds, requestModel);

            // Sort and filter the stories
            bestStories = SortAndFilterStories(bestStories, requestModel);

            responseModel.IsSuccess = true;
            responseModel.Data = bestStories;

            return responseModel;
        }

        private async Task<List<int>> GetBestStoryIdsFromCacheOrApiAsync(string cacheKey)
        {
            if (!_cacheService.TryGetCache<List<int>>(cacheKey, out var bestStoryIds))
            {
                bestStoryIds = await _httpService.GetBestStoryIdsFromApiAsync();
                _cacheService.SetCache(cacheKey, bestStoryIds, TimeSpan.FromMinutes(10)); // Cache the best story IDs
            }

            return bestStoryIds;
        }

        private async Task<List<Story>> FetchStoriesAsync(List<int> storyIds, RequestModel requestModel)
        {
            var startIndex = (requestModel.page - 1) * requestModel.pageSize;
            var endIndex = startIndex + requestModel.pageSize;
            var storyIdsToFetch = storyIds.Skip(startIndex).Take(requestModel.pageSize);

            var bestStories = new List<Story>();

            foreach (var storyId in storyIdsToFetch.Take(requestModel.n))
            {
                var cacheKeyStory = $"Story_{storyId}";
                if (!_cacheService.TryGetCache<Story>(cacheKeyStory, out Story story))
                {
                    story = await _httpService.GetStoryByIdAsync(storyId);
                }

                bestStories.Add(story);
            }

            return bestStories;
        }

        private List<Story> SortAndFilterStories(List<Story> stories, RequestModel requestModel)
        {
            var sortedStories = stories
                .OrderByDescending(story => story.Score)
                .Where(story => string.IsNullOrEmpty(requestModel.searchName) ||
                        story.Title.Contains(requestModel.searchName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return sortedStories;
        }

    }
}
