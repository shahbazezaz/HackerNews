namespace HackerNews.Service
{
    public class LoggerService
    {
        private readonly ILogger<BestStoriesService> _logger;

        public LoggerService(ILogger<BestStoriesService> logger)
        {
            _logger = logger;
        }

        public void LogError(Exception ex, string message)
        {
            _logger.LogError(ex, message);
        }
    }
}
