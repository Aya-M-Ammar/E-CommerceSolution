using E_Commerce.Domain.Entity;
using E_Commerce.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Persistance.SpecificationDesignPattern
{
    internal static class SpecificationEvalutor
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(
            IQueryable<TEntity> query,
            ISpecification<TEntity, TKey> specification)
            where TEntity : BaseEntity<TKey>
        {
            if (specification.Criteria is not null)
            {
                query = query.Where(specification.Criteria);
            }

            query = specification.IncludEx.Aggregate(
                query,
                (currentQuery, include) => currentQuery.Include(include));
          //foreach(var include in specification.IncludEx)
          //  {
          //      query = query.Include(include);
          //  }

            if (specification.OrderBy is not null)
            {
                query = query.OrderBy(specification.OrderBy);
            }

            if (specification.OrderByDescending is not null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.IsPaginated)
            {
                query = query
                    .Skip(specification.Skip)
                    .Take(specification.Take);
            }

            return query;
        }
    }
}