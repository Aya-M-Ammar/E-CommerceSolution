using E_Commerce.Domain.Entity;
using E_Commerce.Domain.Entity.Product;
using E_Commerce.Domain.Interfaces.Repository;
using E_Commerce.Persistance.Data.Contextes;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Persistance.Data.Seeding
{
    public class DataSeed : IDataInitialize
    {
        private readonly StoreDbContext _dbcontext;

        public DataSeed(StoreDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task InitializeASync()
        {
            try
            {
                var HasType = await _dbcontext.Type.AnyAsync();
                var HasBrand = await _dbcontext.Brand.AnyAsync();
                var HasProduct = await _dbcontext.Products.AnyAsync();
                if ((HasType && HasBrand && HasProduct)) return;
                if (!HasType)
                {
                    await SeedJSONFileAsync<ProductType, int>("types.json", _dbcontext.Type);
                }
                if (!HasBrand)
                {
                    await SeedJSONFileAsync<ProductBrand, int>("brands.json", _dbcontext.Brand);
                }
                await _dbcontext.SaveChangesAsync();
                if (!HasProduct)
                {
                    await SeedJSONFileAsync<Product, int>("products.json", _dbcontext.Products);
                }
                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Can't data seed  {ex}");


            }
        }


        async Task SeedJSONFileAsync<T,TKey>(string fileName, DbSet<T> dbSet) where T : BaseEntity<TKey>
        {
            //E-CommerceSolution\Persistance\Data\Seeding\JSONFile\brands.json
            string path = @"..\Persistance\Data\Seeding\JSONFile\" + fileName;
            if(!File.Exists(path)) return;
            try
            {
                //open stream with file ,can read data 
                //can't use File.ReadAllText to allocate data as string in memory 
               using var streem=File.OpenRead(path);
                var data=await JsonSerializer.DeserializeAsync<List<T>>(streem, new JsonSerializerOptions()
                {
                        PropertyNameCaseInsensitive= true
                });
                if (data is not null && data.Count > 0)
                {
                   await dbSet.AddRangeAsync(data);
                    
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Can't data seed  {ex}");
            }


        }
    }
}
