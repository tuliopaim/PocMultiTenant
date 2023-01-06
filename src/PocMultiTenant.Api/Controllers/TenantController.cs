using Microsoft.AspNetCore.Mvc;
using PocMultiTenant.Api.Domain.Entities;
using PocMultiTenant.Api.Infrastructure.Contexts;

namespace PocMultiTenant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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
