using Infrastructure.DataAccess.Attributes;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure.Startup
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSetting<TSetting>(
            this IServiceCollection services, IConfiguration configuration) where TSetting : class, new()
        {
            var type = typeof(TSetting);
            var attribute = type.GetCustomAttribute<ConfigurationNameAttribute>(true);
            var name = attribute == null ? type.Name : attribute.ConfigurationName;

            var setting = configuration.GetRequiredSection(name).Get<TSetting>();
            
            services.AddSingleton(setting);

            return services;
        }

        internal static IServiceCollection AddMapster(this IServiceCollection services)
        {
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;

            services.AddSingleton(typeAdapterConfig);

            services.AddSingleton<IMapper, ServiceMapper>();

            return services;
        }
    }
}
