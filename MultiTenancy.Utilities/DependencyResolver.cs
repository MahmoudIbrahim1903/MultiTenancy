using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiTenancy.Common.Settings;

namespace MultiTenancy.Utilities
{
    public static class DependencyResolver
    {
        public static IServiceCollection AddTenancy(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<TenantSettings>(configuration.GetSection(nameof(TenantSettings)));
            TenantSettings options = new();
            configuration.GetSection(nameof(TenantSettings)).Bind(options);

            return services;
        }
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }
    }
}
