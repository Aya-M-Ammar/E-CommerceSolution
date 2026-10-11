using E_Commerce.Domain.Entity.Identity;
using E_Commerce.Domain.Interfaces.Repository;
using E_Commerce.Persistance.Data.Contextes;
using E_Commerce.Persistance.Data.Seeding;
using E_Commerce.Persistance.Identitydata.DataSeed;
using E_Commerce.Persistance.IdentityData.IdentityContext;
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
using Service_Abstaction.OrderService;
using Service_Abstaction.ProductService;
using Service_Abstaction.PymentService;
using Service_Implementation;
using Service_Implementation.AuthenticationService;
using Service_Implementation.BasketService;
using Service_Implementation.CacheService;
using Service_Implementation.Mapping;
using Service_Implementation.OrderService;
using Service_Implementation.ProductService;
using Service_Implementation.Pyments;
using Shared.DTOS.Pyment;
using StackExchange.Redis;
using System.Text;

namespace E_Commerce_Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container

            builder.Services.AddControllers();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Store DbContext
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Identity DbContext
            builder.Services.AddDbContext<StoreIdentityContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            //// Redis
            builder.Services.AddSingleton<IConnectionMultiplexer>(SP =>
            {
                return ConnectionMultiplexer.Connect(
                    builder.Configuration.GetConnectionString("RedisConnection"));
            });


            builder.Services.AddScoped<ICacheRepository, CacheRepository>();

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(ServiceReference).Assembly);

            // Seed Data
            builder.Services.AddKeyedScoped<IDataInitialize, DataSeed>("defualt");
            builder.Services.AddKeyedScoped<IDataInitialize, DataSeedingIdentity>("Identity");

            // Services
            builder.Services.AddScoped<IUniteOfWork, UnitOfWork>();

            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddScoped<IBasketService, BasketService>();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();

            builder.Services.AddTransient<ProductPictureResolver>();

            builder.Services.AddScoped<ICacheRepository, CacheRepository>();
            builder.Services.AddScoped<ICacheService, CacheService>();

            builder.Services.AddScoped<IAuthenticationUser, AuthenticationUser>();
            builder.Services.AddScoped<IOrderService,OrderService>();

            builder.Services.AddScoped<IPymentService, PymentService>();
            builder.Services.AddSingleton<IPymentGatway, StripPaymentGatway>();


            // Invalid Model State
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory =
                    ApiResponseFactory.CreateInvalidModelStateResponse;
            });

            // Identity
            builder.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityContext>();

            // Authentication - JWT
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;

                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,

                        ValidAudience =
                            builder.Configuration["JWTOptions:Audience"],

                        ValidIssuer =
                            builder.Configuration["JWTOptions:Issuer"],

                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(
                                    builder.Configuration["JWTOptions:SecretKey"]!
                                )
                            )
                    };
            });
            builder.Services.Configure<PaymentGetwaySettings>(builder.Configuration.GetSection("Strip"));

            #endregion

            var app = builder.Build();

            // Database Migration
            await app.MigrateAysnc();

            // Seed Store Data
            await app.SeedAsync();

            // Seed Identity Data
            await app.SeedIdentityAsync();

            #region Configure the HTTP request pipeline

            // Exception Handler
            app.UseMiddleware<ExceptionHandlerMiddleWare>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Authentication MUST come before Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();

            #endregion

            await app.RunAsync();
        }
    }
}