namespace NewsWebApplication.Models
{
    public class MVCFilter
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}