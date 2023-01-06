using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PocMultiTenant.Api.Domain.Entities;
using PocMultiTenant.Api.Infrastructure.Auth;
using PocMultiTenant.Api.Infrastructure.Contexts;
using PocMultiTenant.Api.Infrastructure.Extensions;

namespace PocMultiTenant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    [HttpPost]
    public async Task<IActionResult> CreateUser(
        [FromServices] AdminDbContext adminDbContext,
        [FromServices] IConfiguration configuration,
        [FromBody] User user)
    {
        var tenantExist = await adminDbContext.Tenants.AnyAsync(t => t.Id == user.Tenant);

        if (!tenantExist)
        {
            return BadRequest(new { Error = "Tenant not found!" });
        }

        var dbContext = configuration.BuildPocDbContext(user.Tenant);

        dbContext.Add(user);

        await dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Users([FromServices] PocDbContext dbContext)
    {
        return Ok(await dbContext.Users.ToListAsync());
    }

    [HttpGet("login")]
    public async Task<IActionResult> UserLogin(
        [FromServices] AdminDbContext adminDbContext,
        [FromServices] IConfiguration configuration,
        [FromQuery] int userId,
        [FromQuery] int tenantId)
    {
        var tenantExist = await adminDbContext.Tenants.AnyAsync(t => t.Id == tenantId);

        if (!tenantExist)
        {
            return BadRequest(new { Error = "Tenant not found!" });
        }

        var dbContext = configuration.BuildPocDbContext(tenantId);

        var user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId && u.Tenant == tenantId);

        if (user == null)
        {
            return BadRequest(new { Error = "User not found!" });
        }

        var token = TokenGenerator.GenerateToken(new GenerateTokenDto
        {
            User = user,
            ExpirationTime = TimeSpan.FromHours(6),
            Secret = configuration["JwtSecret"]!
        });

        return Ok(new { Token = token});
    }
}
