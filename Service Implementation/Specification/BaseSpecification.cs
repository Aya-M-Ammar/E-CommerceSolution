using E_Commerce.Domain.Entity;
using E_Commerce.Domain.Interfaces.Repository;
using System.Linq.Expressions;

namespace Service_Abstaction.Specification
{
    internal abstract class BaseSpacification<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public ICollection<Expression<Func<TEntity, object>>> IncludEx { get; } = [];

        public Expression<Func<TEntity, bool>> Criteria { get; }

        public Expression<Func<TEntity, object>> OrderBy { get; private set; }

        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPaginated { get; private set; }
        protected void ApplyPagineted(int PageSize, int pageindex)
        {
            IsPaginated = true;
            Take = PageSize;
            Skip = (pageindex - 1) * PageSize;
        }

        protected BaseSpacification(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }

        public void AddInclude(Expression<Func<TEntity, object>> includeex)
        {
            IncludEx.Add(includeex);

        }
       

        public void Addorder(Expression<Func<TEntity, object>> orderexpression)
        {
            OrderBy = orderexpression;
        }
        public void AddorderDescinding(Expression<Func<TEntity, object>> orderexpression)
        {
            OrderByDescending = orderexpression;
        }
    }
}