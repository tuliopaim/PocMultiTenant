using Microsoft.EntityFrameworkCore;
using PocMultiTenant.Api.Domain.Entities;

namespace PocMultiTenant.Api.Infrastructure;

public class PocDbContext : DbContext
{
    public PocDbContext(DbContextOptions<PocDbContext> config) : base(config)
    { }

    public DbSet<ToDo> ToDos => Set<ToDo>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Tenant).IsRequired().IsRequired();
            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<ToDo>(builder =>
        {
            builder.ToTable("ToDos");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Title).HasMaxLength(150).IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.ToDos)
                .HasForeignKey(x => x.UserId);
        });
    }
}
