using E_Commerce.Domain.Interfaces.Repository;
using E_Commerce.Persistance.Data.Contextes;
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
            var pindingMigrations = await contextservices.Database.GetPendingMigrationsAsync();
            if (pindingMigrations.Any())
                await contextservices.Database.MigrateAsync();
            return app;


        }
        public static async Task<WebApplication> SeedAsync(this WebApplication app)
        {
           await using var scope = app.Services.CreateAsyncScope();
            var Initializer = scope.ServiceProvider.GetRequiredService<IDataInitialize>();
            await Initializer.InitializeASync();
            return app;
        }
    }
}