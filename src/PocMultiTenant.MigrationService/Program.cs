using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PocMultiTenant.Api.Infrastructure;
using PocMultiTenant.MigrationService;

var configuration = SetupConfiguration();
var serviceProvider = SetupDependencyInjection();
var cancellationToken = SetupCancellationToken();

using var scope = serviceProvider.CreateScope();
var migrator = scope.ServiceProvider.GetRequiredService<Migrator>();

await migrator.Migrate(cancellationToken);

IConfigurationRoot SetupConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", optional: true, reloadOnChange: true); 
    return builder.Build();
}

IServiceProvider SetupDependencyInjection()
{
    IServiceCollection services = new ServiceCollection();

    services.AddLogging();
    services.AddSingleton<IConfiguration>(_ => configuration);

    services.AddPocDbContext(configuration);
    services.AddAdminDbContext(configuration);

    services.AddScoped<Migrator>();

    return services.BuildServiceProvider();
}

CancellationToken SetupCancellationToken()
{
    var cts = new CancellationTokenSource();

    Console.CancelKeyPress += (sender, eventArgs) =>
    {
        cts.Cancel();
        eventArgs.Cancel = true;
    };

    return cts.Token;
}
