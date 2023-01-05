using Microsoft.AspNetCore.Mvc;
using PocMultiTenant.Api.Domain.Entities;
using PocMultiTenant.Api.Infrastructure;

namespace PocMultiTenant.Api.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CriarUsuario(
        [FromServices] PocDbContext dbContext,
        [FromBody] User user)
    {
        dbContext.Add(user);

        await dbContext.SaveChangesAsync();

        return Ok();
    }
}
