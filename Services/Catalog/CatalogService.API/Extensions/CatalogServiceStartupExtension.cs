using CatalogService.API.Services;
using CatalogService.API.Services.Abstraction;
using CatalogService.Infrastructure.DataAccess.DbContexts;
using CatalogService.Infrastructure.DataAccess.DbContexts.Abstraction;
using CatalogService.Infrastructure.Repositories;
using CatalogService.Infrastructure.Repositories.Abstraction;
using CatalogService.Infrastructure.UnitOfWorks;
using CatalogService.Infrastructure.UnitOfWorks.Abstraction;
using Global.Shared.Settings;
using Infrastructure.DataAccess.Abstraction;
using Infrastructure.DataAccess.DbContexts;
using Infrastructure.Startup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.API.Extensions
{
    public static class CatalogServiceStartupExtension
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddSetting<NonRelationalDatabaseSetting>(configuration)
                .AddScoped<INonRelationalDbContext, NonRelationalDbContext>()
                .AddScoped<ICatalogDbContext, CatalogDbContext>()
                .AddScoped<IProductRepository, ProductRepository>()
                .AddScoped<ICatalogUnitOfWork, CatalogUnitOfWork>()
                .AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}
