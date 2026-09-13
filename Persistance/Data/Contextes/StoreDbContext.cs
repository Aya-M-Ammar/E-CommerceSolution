using E_Commerce.Domain.Entity.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistance.Data.Contextes
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); }        
    
  
    public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> Brand { get; set; }
        public DbSet<ProductType> Type { get; set; }
    }

    }
