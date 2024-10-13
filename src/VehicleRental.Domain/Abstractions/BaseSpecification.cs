using System.Linq.Expressions;

namespace VehicleRental.Domain.Abstractions
{
    public abstract class BaseSpecification<TEntity, TEntityId> : ISpecification<TEntity, TEntityId> where TEntity : Entity<TEntityId> where TEntityId : class
    {
        protected BaseSpecification() { }

        protected BaseSpecification(Expression<Func<TEntity, bool>>? criteria) => Criteria = criteria;

        public Expression<Func<TEntity, bool>>? Criteria { get; }

        public List<Expression<Func<TEntity, object>>> Includes { get; } = [];

        public Expression<Func<TEntity, object>>? OrderBy { get; private set; }

        public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagingEnable { get; private set; }

        protected void AddOrderBy(Expression<Func<TEntity, object>> expression)
        {
            OrderBy = expression;
        }

        protected void AddOrderByDescending(Expression<Func<TEntity, object>> expression)
        {
            OrderByDescending = expression;
        }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnable = true;
        }

        protected void AddInclude(Expression<Func<TEntity, object>> expression)
        {
            Includes.Add(expression);
        }

    }
}
