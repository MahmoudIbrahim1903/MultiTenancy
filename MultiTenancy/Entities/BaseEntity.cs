namespace MultiTenancy.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string TenantId { get; set; } = null!;
    }
}
