using Microsoft.EntityFrameworkCore;
using server_dotnet_dal.Entities;

namespace server_dotnet_dal.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Organization> Organizations { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.FirstName)
                .IsRequired();
            entity.Property(u => u.LastName)
                .IsRequired();
            entity.Property(u => u.Email)
                .IsRequired();
            entity.Property(u => u.DateCreated)
                .IsRequired();
            entity.HasOne(u => u.Organization)
                .WithMany(o => o.Users)
                .HasForeignKey(u => u.OrganizationId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Name)
                .IsRequired();
            entity.Property(o => o.Industry);
            entity.Property(o => o.DateFounded)
                .IsRequired();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.OrderDate)
                .IsRequired();
            entity.Property(o => o.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            entity.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(o => o.Organization)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.OrganizationId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });

        base.OnModelCreating(modelBuilder);
    }
}
