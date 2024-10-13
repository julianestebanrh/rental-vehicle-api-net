using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using VehicleRental.Domain.Abstractions;
using VehicleRental.Infrastructure.Extensions;
using VehicleRental.Infrastructure.Specifications;

namespace VehicleRental.Infrastructure.Repositories
{
    internal abstract class Repository<TEntity, TEntityId> where TEntity : Entity<TEntityId> where TEntityId : class
    {
        protected readonly ApplicationDbContext DbContext;

        protected Repository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
        {
            return await DbContext
                .Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public virtual void Add(TEntity entity)
        {
            DbContext.Add(entity);
        }

        public async Task<int> CountAsync(ISpecification<TEntity, TEntityId> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).CountAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecification<TEntity, TEntityId> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity, TEntityId> specification)
        {
            var query = DbContext.Set<TEntity>().AsQueryable();
            return SpecificationEvaluator<TEntity, TEntityId>.GetQuery(query, specification);
        }

        public IQueryable<TEntity> GetAllAsync(Expression<Func<TEntity, bool>>? predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes, int page, int pageSize, string orderBy, bool ascending, bool disableTracking = true)
        {
            IQueryable<TEntity> queryable = DbContext.Set<TEntity>();

            if (disableTracking) queryable = queryable.AsNoTracking();

            if (predicate is not null) queryable = queryable.Where(predicate);

            if (includes is not null) queryable = includes(queryable);

            if (!string.IsNullOrEmpty(orderBy))
            {
                queryable = queryable.OrderByPropertyOrField(orderBy, ascending);

            }

            return queryable;
        }

    }
}
