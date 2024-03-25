using MultiTenancy.Contracts;

namespace MultiTenancy.Entities
{
    public class Product : IMustHaveTenant
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Rate { get; set; }
        public string TenantId { get; set; } = null!;
    }
}
