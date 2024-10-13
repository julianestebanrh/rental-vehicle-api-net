namespace VehicleRental.Domain.Abstractions
{
    public class PagedResult<TEntity, TEntityId> where TEntity : Entity<TEntityId> where TEntityId : class
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalNumberOfPages { get; set; }
        public int TotalNumberOfItems { get; set; }
        public List<TEntity> Items { get; set; } = new();
    }
}
