
using MultiTenancy.Common.Settings;

namespace MultiTenancy.Services
{
    public interface ITenantService
    {
        string GetDatabaseProvider();
        Tenant? GetCurrentTenant();
        string GetConnectionString();
    }
}