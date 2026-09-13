using E_Commerce.Domain.Entity;
using System.Linq.Expressions;

namespace E_Commerce.Domain.Interfaces.Repository
{
    public interface ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>

    {
        public ICollection<Expression<Func<TEntity, object>>> IncludEx { get; }
        public Expression<Func<TEntity, bool>> Criteria { get; }
        public Expression<Func<TEntity, object>> OrderBy { get; }
        public Expression<Func<TEntity, object>> OrderByDescending { get; }
        public int Take { get; }
        public int Skip { get; }
        public bool IsPaginated { get; }






    }
}