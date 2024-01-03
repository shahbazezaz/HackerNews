using Microsoft.AspNetCore.Mvc;
using HackerNews.Service;
using HackerNews.Model;

namespace HackerNews.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BestStoriesController : ControllerBase
    {
        private readonly BestStoriesService _bestStoriesService;

        public BestStoriesController(BestStoriesService bestStoriesService)
        {
            _bestStoriesService = bestStoriesService;
        }

        [HttpPost]
        public async Task<ResponseModel> GetBestStories(RequestModel requestModel)
        {
            return await _bestStoriesService.GetBestStoryIdsAsync(requestModel);
        }


    }
}
