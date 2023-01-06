using PocMultiTenant.Api.Domain.Entities;

namespace PocMultiTenant.Api.Infrastructure.Auth;

public class GenerateTokenDto
{
    public required User User { get; set; }
    public required TimeSpan ExpirationTime { get; set; }
    public required string Secret { get; set; }
}
