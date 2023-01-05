namespace PocMultiTenant.Api.Domain.Entidades;

public class ToDo
{
    public int Id { get; set; }
    public required string Title { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }
}
