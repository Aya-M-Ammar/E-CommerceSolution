using E_Commerce.Domain.Entity;
using E_Commerce.Domain.Interfaces.Repository;
using E_Commerce.Persistance.Data.Contextes;
using E_Commerce.Persistance.SpecificationDesignPattern;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Persistance.Repository
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _dbcontext;

        public GenericRepository(StoreDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbcontext.Set<TEntity>().AddAsync(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbcontext.Set<TEntity>().ToListAsync();
        }

     
        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> specification)
        {
            var query = SpecificationEvalutor.CreateQuery(_dbcontext.Set<TEntity>(), specification);
            return await query.ToListAsync();
        }
        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await _dbcontext.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> specification)
        {
            return await SpecificationEvalutor
                .CreateQuery(_dbcontext.Set<TEntity>(), specification)
                .FirstOrDefaultAsync();
        }

        public void Remove(TEntity entity)
        {
            _dbcontext.Set<TEntity>().Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _dbcontext.Set<TEntity>().Update(entity);
        }
    }
}