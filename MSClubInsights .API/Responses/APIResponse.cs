using System.Net;

namespace MSClubInsights.API.Responses
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccess { get; set; }

        public List<string> ErrorMessages { get; set; }

        public object Data { get; set; }
    }
}
