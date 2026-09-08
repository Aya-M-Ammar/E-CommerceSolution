using E_Commerce.Domain.Entity;
using E_Commerce.Domain.Interfaces.Repository;
using E_Commerce.Persistance.Data.Contextes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistance.Repository
{
    public class UnitOfWork : IUniteOfWork
    {
        private readonly StoreDbContext _dbcontext;
       
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(StoreDbContext dbcontext)
        {
           _dbcontext = dbcontext;
        }
        public IGenericRepository<TEntity, TKey> GetRepositoryAsync<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            if (_repositories.TryGetValue(typeof(TEntity), out var repository))
            {
                return (IGenericRepository<TEntity, TKey>)repository;
            }
            var newRepository = new GenericRepository<TEntity, TKey>(_dbcontext);
            _repositories.Add(typeof(TEntity), newRepository);
            return newRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbcontext.SaveChangesAsync();
        }
    }
}
