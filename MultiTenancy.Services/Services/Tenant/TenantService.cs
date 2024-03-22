using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MultiTenancy.Common.Consts;
using MultiTenancy.Common.Settings;

namespace MultiTenancy.Services
{
    public class TenantService : ITenantService
    {
        private TenantSettings _tenantSettings;
        private HttpContext? _httpContext;
        private Tenant? _currentTenant;
        public TenantService(IHttpContextAccessor httpContextAccessor, IOptions<TenantSettings> tenantSettings)
        {
            _tenantSettings = tenantSettings.Value;
            _httpContext = httpContextAccessor.HttpContext;

            if (_httpContext is not null)
            {
                if (_httpContext.Request.Headers.TryGetValue(TenantConsts.TenantIdHeaderKey, out var tenantId))                
                    SetCurrentTenant(tenantId!);
                
                else
                    throw new Exception("Missing tenantId key");
            }
        }

        public string GetConnectionString()
        {
            return _currentTenant is not null ? _currentTenant.TenantConnectionString! :
                                                _tenantSettings.Defaults.DefaultConnectionString;
        }

        public Tenant? GetCurrentTenant()
            => _currentTenant;

        public string GetDatabaseProvider()
            => _tenantSettings.Defaults.DbProvider;

        #region Helpers
        private void SetCurrentTenant(string tenantId)
        {
            _currentTenant = _tenantSettings.Tenants.FirstOrDefault(t => t.TenantId == tenantId);

            if (_currentTenant is null)
                throw new Exception($"Invalid TenantId: {tenantId}");

            if (string.IsNullOrEmpty(_currentTenant.TenantConnectionString))
                _currentTenant.TenantConnectionString = _tenantSettings.Defaults.DefaultConnectionString;
        }
        #endregion
    }
}
