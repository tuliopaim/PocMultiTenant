using Microsoft.EntityFrameworkCore;
using PocMultiTenant.Api.Domain.Entidades;

namespace PocMultiTenant.Api.Infrastructure;

public class AdminDbContext : DbContext
{
    public AdminDbContext(DbContextOptions<AdminDbContext> config) : base(config)
    { }

    public DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tenant>(builder =>
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever();

            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

            builder.HasData(
                new Tenant
                {
                    Id = 1,
                    Name = "Tenant-1"
                },
                new Tenant
                {
                    Id = 2,
                    Name = "Tenant-2"
                });
        });
    }
}
