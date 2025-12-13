using Payment.Gateway.DTOs;

namespace Payment.Gateway.Services.Common
{
    public static class PageListRequestValidator
    {
        public static void Normalize(TransactionListRequestDto request)
        {
            request.Page = request.Page < 1 ? 1 : request.Page;
            request.PageSize = request.PageSize switch
            {
                < 1 => 10,
                > 100 => 100,
                _ => request.PageSize
            };
        }
    }
}
