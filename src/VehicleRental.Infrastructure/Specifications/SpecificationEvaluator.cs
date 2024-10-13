using Microsoft.EntityFrameworkCore;
using VehicleRental.Domain.Abstractions;

namespace VehicleRental.Infrastructure.Specifications
{
    public class SpecificationEvaluator<TEntity, TEntityId> where TEntity : Entity<TEntityId> where TEntityId : class
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> query, ISpecification<TEntity, TEntityId> specification)
        {
            if (specification.Criteria is not null)
            {
                query = query.Where(specification.Criteria);
            }

            if (specification.OrderBy is not null)
            {
                query = query.OrderBy(specification.OrderBy);
            }

            if (specification.OrderByDescending is not null)
            {
                query = query.OrderBy(specification.OrderByDescending);
            }

            if (specification.IsPagingEnable)
            {
                query = query.Skip(specification.Skip).Take(specification.Take);
            }

            query = specification.Includes!.Aggregate(query, (current, include) => current.Include(include)).AsSplitQuery().AsNoTracking();

            return query;
        }
    }
}
