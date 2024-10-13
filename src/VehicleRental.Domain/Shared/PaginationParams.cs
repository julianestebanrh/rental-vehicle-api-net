namespace VehicleRental.Domain.Shared
{
    public record PaginationParams
    {
        private const int _maxPageSize = 50;
        private int _pageSize = 20;

        public string? Sort { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get => _pageSize; set => _pageSize = (value > _maxPageSize) ? _maxPageSize : value; }
        public string? OrderBy { get; set; }
        public bool OrderAsc { get; set; } = true;
        public string? Search { get; set; }
    }
}
