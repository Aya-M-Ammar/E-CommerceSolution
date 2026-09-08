using E_Commerce.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Interfaces.Repository
{
    public interface IUniteOfWork
    {
        Task<int> SaveChangesAsync();
        IGenericRepository<TEntity, TKey> GetRepositoryAsync<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
    }
}
