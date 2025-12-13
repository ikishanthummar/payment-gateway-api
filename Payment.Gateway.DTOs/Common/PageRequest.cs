namespace Payment.Gateway.DTOs.Common
{
    public class PageRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;

        public string? Search { get; set; }

        public string? SortBy { get; set; } = "createdOn";
        public bool IsDescending { get; set; } = true;
    }
}
