namespace VehicleRental.Domain.Abstractions
{
    public class PagedResponse<TEntity, TEntityId> where TEntity : Entity<TEntityId> where TEntityId : class
    {
        public int Count { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public IReadOnlyList<TEntity>? Items { get; set; }
        public int PageCount { get; set; }
        public int ItemByPage { get; set; }
    }
}
