using CatalogService.API.Extensions;
using Infrastructure.Startup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.API
{
    public class Startup : BaseApiStartup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void ConfigureCustomServices(IServiceCollection services)
        {
            services.AddInfrastructureServices(Configuration);
        }
    }
}
