namespace PocMultiTenant.Api.Domain.Entities;

public class ToDo
{
    public int Id { get; set; }
    public required string Title { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }
}
