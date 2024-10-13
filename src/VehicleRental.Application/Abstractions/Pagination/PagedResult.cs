using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace VehicleRental.Application.Abstractions.Pagination
{
    public sealed class PagedResult<TDestination>
    {
        private PagedResult(List<TDestination> items, int page, int pageSize, int totalCount, int totalOfPages)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalOfPages = totalOfPages;
        }

        public List<TDestination> Items { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public int TotalOfPages { get; }
        public bool HasNextPage => Page * PageSize > TotalCount;
        public bool HasPreviousPage => PageSize > 1;

        public static async Task<PagedResult<TDestination>> CreateAsync<TSource>(IQueryable<TSource> query, int page, int pageSize, IMapper mapper)
        {
            var totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var mod = totalCount % pageSize;
            var totalOfPages = (totalCount / pageSize) + (mod == 0 ? 0 : 1);

            var itemsMap = mapper.Map<List<TDestination>>(items);

            return new(itemsMap, page, pageSize, totalCount, totalOfPages);
        }
    }
}
