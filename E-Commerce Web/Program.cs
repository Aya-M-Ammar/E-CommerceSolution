using E_Commerce.Domain.Entity.IDentity;
using E_Commerce.Domain.Interfaces.Repository;
using E_Commerce.Persistance.Data.Contextes;
using E_Commerce.Persistance.Data.Seeding;
using E_Commerce.Persistance.IdentityData.DataSeed;
using E_Commerce.Persistance.IdentityData.IDentityContext;
using E_Commerce.Persistance.Repository;
using E_Commerce_Web.ExciptionHandeler;
using E_Commerce_Web.Extentions;
using E_Commerce_Web.Factories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Service_Abstaction.AuthinticationService;
using Service_Abstaction.BasketService;
using Service_Abstaction.CacheService;
using Service_Abstaction.ProductService;
using Service_Implementation;
using Service_Implementation.AuthenticationService;
using Service_Implementation.BasketService;
using Service_Implementation.CacheService;
using Service_Implementation.Mapping;
using Service_Implementation.ProductService;
using StackExchange.Redis;
using System.Text;
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
            builder.Services.AddDbContext<StoreIdentityContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));

            });
            builder.Services.AddSingleton<IConnectionMultiplexer>(SP =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection"));
            });
            //Mapping
            builder.Services.AddAutoMapper(typeof(ServiceReference).Assembly);
            //Seed Data
            builder.Services.AddKeyedScoped<IDataInitialize, DataSeed>("defualt");
            builder.Services.AddKeyedScoped<IDataInitialize, DataSeedingIdentity>("Identity");

            //services
            builder.Services.AddScoped<IUniteOfWork, UnitOfWork>();
           

            builder.Services.AddScoped<IProductService,ProductService>();
            builder.Services.AddScoped<IBasketService, BasketService>();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddTransient<ProductPictureResolver>();
            builder.Services.AddScoped<ICacheRepository, CacheRepository>();
            builder.Services.AddScoped<ICacheService, CacheService>();
            builder.Services.AddScoped<IAuthenticationUser, AuthenticationUser>();
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.CreateInvalidModelStateResponse;
            });

            builder.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityContext>();
            builder.Services.AddAuthentication(option =>
            {

                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidAudience = builder.Configuration["JWTOption:Audience"],
                        ValidIssuer = builder.Configuration["JWTOption:Issuer"],
                    //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTOption:SecretKey"]))
                    }
                    ;
                });
           
                



            #endregion
            var app = builder.Build();
          await  app.MigrateAysnc();
          await  app.SeedAsync();
         await   app.SeedIdentityAsync();

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
