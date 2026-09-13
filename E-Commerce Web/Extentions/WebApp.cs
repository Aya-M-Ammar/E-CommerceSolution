using E_Commerce.Domain.Interfaces.Repository;
using E_Commerce.Persistance.Data.Contextes;
using E_Commerce.Persistance.IdentityData.IDentityContext;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace E_Commerce_Web.Extentions
{
    public static class WebApp
    {
        public static async Task<WebApplication> MigrateAysnc(this WebApplication app)
        {
            await using var scope =  app.Services.CreateAsyncScope();
            var contextservices = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            var contextservices2 = scope.ServiceProvider.GetRequiredService<StoreIdentityContext>();

            var pindingMigrations = await contextservices.Database.GetPendingMigrationsAsync();
            var pindingMigrations2 = await contextservices2.Database.GetPendingMigrationsAsync();

            if (pindingMigrations.Any())
                await contextservices.Database.MigrateAsync();
            if (pindingMigrations2.Any())
                await contextservices2.Database.MigrateAsync();
            return app;


        }
        public static async Task<WebApplication> SeedAsync(this WebApplication app)
        {
           await using var scope = app.Services.CreateAsyncScope();
            var Initializer = scope.ServiceProvider.GetRequiredKeyedService<IDataInitialize>("defualt");
            await Initializer.InitializeASync();
            return app;
        }
        public static async Task<WebApplication> SeedIdentityAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var Initializer = scope.ServiceProvider.GetRequiredKeyedService<IDataInitialize>("Identity");
            await Initializer.InitializeASync();
            return app;
        }
    }
}