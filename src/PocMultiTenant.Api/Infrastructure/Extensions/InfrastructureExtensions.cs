using Microsoft.EntityFrameworkCore;
using PocMultiTenant.Api.Configuration;
using PocMultiTenant.Api.Infrastructure.Contexts;

namespace PocMultiTenant.Api.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    private const string TenantVariable = "{0}";

    public static IServiceCollection AddPocDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddScoped(provider =>
        {
            var currentUser = provider.GetRequiredService<ICurrentUser>();
            var tenantId = currentUser.Tenant();

            return configuration.BuildPocDbContext(tenantId);
        });
    }

    public static PocDbContext BuildPocDbContext(this IConfiguration configuration, int tenantId)
    {
        var connectionString = configuration.GetTenantConnectionString(tenantId);
        var optionsBuilder = new DbContextOptionsBuilder<PocDbContext>();

        optionsBuilder.UseNpgsql(connectionString);

        return new PocDbContext(optionsBuilder.Options);
    }

    public static string GetTenantConnectionString(this IConfiguration configuration, int tenantId)
    {
        var templateConnection = configuration.GetConnectionString($"TemplateConnection");

        if (string.IsNullOrEmpty(templateConnection))
        {
            throw new Exception("TemplateConnection not registered!");
        }

        return templateConnection!.Replace(TenantVariable, tenantId.ToString());
    }

    public static IServiceCollection AddAdminDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDbContext<AdminDbContext>(opt =>
            opt.UseNpgsql(configuration.GetConnectionString("AdminConnection")));
    }
}
