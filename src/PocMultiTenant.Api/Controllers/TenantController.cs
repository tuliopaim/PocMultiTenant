using Microsoft.AspNetCore.Mvc;
using PocMultiTenant.Api.Domain.Entidades;
using PocMultiTenant.Api.Infrastructure;

namespace PocMultiTenant.Api.Controllers;

[ApiController]
public class TenantController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CriarTenant(
        [FromServices] AdminDbContext dbContext,
        [FromBody] Tenant tenant)
    {
        dbContext.Add(tenant);

        await dbContext.SaveChangesAsync();

        return Ok();
    }
}
