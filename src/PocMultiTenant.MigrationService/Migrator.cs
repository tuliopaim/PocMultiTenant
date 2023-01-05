using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PocMultiTenant.Api.Infrastructure;

namespace PocMultiTenant.MigrationService;

public class Migrator
{
    private readonly ILogger<Migrator> _logger;
    private readonly AdminDbContext _adminDbContext;
    private readonly IConfiguration _configuration;

    public Migrator(
        ILogger<Migrator> logger,
        AdminDbContext adminDbContext,
        IConfiguration configuration)
    {
        _logger = logger;
        _adminDbContext = adminDbContext;
        _configuration = configuration;
    }

    public async Task Migrate(CancellationToken stoppingToken)
    {
        var tenants = await _adminDbContext.Tenants.ToListAsync(stoppingToken);

        foreach(var tenant in tenants)
        {
            if (stoppingToken.IsCancellationRequested) return;
            try
            {
                var pocDbContext = ObterPocDbContext(tenant.Id);

                await pocDbContext.Database.MigrateAsync(cancellationToken: stoppingToken);

                _logger.LogInformation(
                    "Migrations applied for Tenant [{TenantId} - {TenantName}]",
                    tenant.Id,
                    tenant.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Exception captured on the migration of the Tenant [{TenantId} - {TenantName}]",
                    tenant.Id,
                    tenant.Name);
            }
        }
    }

    private PocDbContext ObterPocDbContext(int tenantId)
    {
        var connectionString = _configuration.GetTenantConnectionString(tenantId);
        var optionsBuilder = new DbContextOptionsBuilder<PocDbContext>();

        optionsBuilder.UseNpgsql(connectionString);

        return new PocDbContext(optionsBuilder.Options);
    }
}
