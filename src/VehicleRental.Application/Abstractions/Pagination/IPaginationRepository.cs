using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using VehicleRental.Domain.Abstractions;

namespace VehicleRental.Application.Abstractions.Pagination
{
    public interface IPaginationRepository<TEntity, TEntityId> where TEntity : Entity<TEntityId> where TEntityId : class
    {
        //Task<PagedResult<TEntity, TEntityId>> GetAllAsync(
        //    Expression<Func<TEntity, bool>> predicate,
        //    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes,
        //    int page,
        //    int pageSize,
        //    string orderBy,
        //    bool isAscending = true,
        //    bool disableTracking = true
        //);

        IQueryable<TEntity> GetAllAsync(Expression<Func<TEntity, bool>>? predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes, int page, int pageSize, string orderBy, bool ascending, bool disableTracking = true);

    }

}
