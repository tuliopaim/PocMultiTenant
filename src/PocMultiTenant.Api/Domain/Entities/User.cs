namespace PocMultiTenant.Api.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public int Tenant { get; set; }
    public required string Name { get; set; }

    public List<ToDo> ToDos { get; set; } = new();
}
