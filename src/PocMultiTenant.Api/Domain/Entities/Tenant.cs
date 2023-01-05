namespace PocMultiTenant.Api.Domain.Entities;

public class Tenant
{
    public int Id { get; set; }
    public required string Name { get; set; }
}
