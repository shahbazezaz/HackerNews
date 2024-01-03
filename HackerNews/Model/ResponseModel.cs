namespace HackerNews.Model
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public List<Story> Data { get; set; }
    }
}
