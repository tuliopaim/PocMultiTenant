using Microsoft.AspNetCore.Http;
using PocMultiTenant.Api.Core.Constants;

namespace PocMultiTenant.Api.Core;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAcessor;

    public CurrentUser(IHttpContextAccessor httpContextAcessor)
    {
        _httpContextAcessor = httpContextAcessor;
    }

    public int Id()
    {
        var claim = _httpContextAcessor.HttpContext?.User?.FindFirst(PocClaims.Id);

        return int.TryParse(claim?.Value, out var id) ? id : 0;
    }

    public int Tenant()
    {
        var claim = _httpContextAcessor.HttpContext?.User?.FindFirst(PocClaims.Tenant);

        return int.TryParse(claim?.Value, out var tenant) ? tenant : 0;
    }
}
