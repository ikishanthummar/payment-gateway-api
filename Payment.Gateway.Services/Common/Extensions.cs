using Microsoft.EntityFrameworkCore;
using Payment.Gateway.DTOs.Common;
using System.Linq.Expressions;

namespace Payment.Gateway.Services.Common
{
    public static class Extensions
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        int page,
        int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var totalRecords = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<T>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Items = items
            };
        }

        public static IQueryable<T> ApplySort<T>(
            this IQueryable<T> query,
            string? sortBy,
            bool isDescending,
            IDictionary<string, Expression<Func<T, object>>> sortMap,
            Expression<Func<T, object>> defaultSort)
        {
            if (string.IsNullOrWhiteSpace(sortBy) || !sortMap.ContainsKey(sortBy))
            {
                return isDescending
                    ? query.OrderByDescending(defaultSort)
                    : query.OrderBy(defaultSort);
            }

            var sortExpression = sortMap[sortBy];

            return isDescending
                ? query.OrderByDescending(sortExpression)
                : query.OrderBy(sortExpression);
        }
    }
}
