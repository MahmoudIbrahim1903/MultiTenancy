
namespace MultiTenancy.Common.Settings
{
    public class Tenant
    {
        public string TenantName { get; set; } = null!;
        public string TenantId { get; set; } = null!;
        public string? TenantConnectionString { get; set; }
    }
}
