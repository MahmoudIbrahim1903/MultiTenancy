using Microsoft.EntityFrameworkCore;
using MultiTenancy.Repositories;
using MultiTenancy.Services;

namespace MultiTenancy.Utilities
{
    public static class DependencyResolver
    {
        public static IServiceCollection RegisterDbContext(this IServiceCollection services, ConfigurationManager configuration)
        {
            //configure settings class
            services.Configure<TenantSettings>(configuration.GetSection(nameof(TenantSettings)));
            TenantSettings options = new();
            configuration.GetSection(nameof(TenantSettings)).Bind(options);

            //Register DbConext
            var dbProvider = options.Defaults.DbProvider;
            if (dbProvider?.ToLower() == "mssql")
            {
                services.AddDbContext<ApplicationDbContext>(m => m.UseSqlServer());
            }

            //migrate all databases
            foreach (var tenant in options.Tenants)
            {
                var connectionString = tenant.TenantConnectionString ?? options.Defaults.DefaultConnectionString;

                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                dbContext.Database.SetConnectionString(connectionString);
                if (dbContext.Database.GetPendingMigrations().Any())
                    dbContext.Database.Migrate();
            }

            return services;
        }
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
