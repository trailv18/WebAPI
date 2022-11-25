namespace WebAPI.Core.Models
{
    public class Response
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Data { get; internal set; }
    }
}
