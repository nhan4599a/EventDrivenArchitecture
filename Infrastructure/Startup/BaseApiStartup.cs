using Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Infrastructure.Startup
{
    public abstract class BaseApiStartup
    {
        protected IConfiguration Configuration { get; set; }

        public BaseApiStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected virtual void ConfigureCustomServices(IServiceCollection services)
        {
        }

        private Action<IApplicationBuilder, IWebHostEnvironment>? CustomErrorsHandlingCallback;

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMapster();
            ConfigureCustomServices(services);
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (CustomErrorsHandlingCallback != null)
            {
                CustomErrorsHandlingCallback(app, env);
            }
            else
            {
                app.UseMiddleware<DefaultExceptionHandlingMiddleware>();
            }
            app.UseHsts();
            app.UseHttpsRedirection();


            app.UseRouting();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void RegisterCustomErrosHandlingCallback(Action<IApplicationBuilder, IWebHostEnvironment> callback)
        {
            CustomErrorsHandlingCallback = callback;
        }
    }
}
