using E_Commerce.Domain.Interfaces.Repository;
using E_Commerce.Persistance.Data.Contextes;
using E_Commerce.Persistance.Data.Seeding;
using E_Commerce.Persistance.Repository;
using E_Commerce_Web.ExciptionHandeler;
using E_Commerce_Web.Extentions;
using E_Commerce_Web.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service_Abstaction.BasketService;
using Service_Abstaction.CacheService;
using Service_Abstaction.ProductService;
using Service_Implementation;
using Service_Implementation.BasketService;
using Service_Implementation.CacheService;
using Service_Implementation.Mapping;
using Service_Implementation.ProductService;
using StackExchange.Redis;
using System.Threading.Tasks;
namespace E_Commerce_Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container.
            

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //DbContextService
            builder.Services.AddDbContext<StoreDbContext>(options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

                });
            builder.Services.AddSingleton<IConnectionMultiplexer>(SP =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection"));
            });
            //Mapping
            builder.Services.AddAutoMapper(typeof(ServiceReference).Assembly);
            //Seed Data
            builder.Services.AddScoped<IDataInitialize, DataSeed>();
            //services
          builder.Services.AddScoped<IUniteOfWork, UnitOfWork>();
           

            builder.Services.AddScoped<IProductService,ProductService>();
            builder.Services.AddScoped<IBasketService, BasketService>();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddTransient<ProductPictureResolver>();
            builder.Services.AddScoped<ICacheRepository, CacheRepository>();
            builder.Services.AddScoped<ICacheService, CacheService>();
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.CreateInvalidModelStateResponse;
            });




            #endregion
            var app = builder.Build();
          await  app.MigrateAysnc();
          await  app.SeedAsync();

            #region Configure the HTTP request pipeline.
            app.UseMiddleware<ExciptionHandelerMiddelWare>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();

            #endregion
            await app.RunAsync();
        }
    }
}
