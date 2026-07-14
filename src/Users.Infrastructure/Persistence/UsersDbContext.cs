using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;

namespace Users.Infrastructure.Persistence;

public class UsersDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null;
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(b =>
        {
            b.HasKey(u => u.Id);
            b.HasIndex(u => u.Email).IsUnique();
            b.Property(u => u.Name).IsRequired().HasMaxLength(200);
            b.Property(u => u.Email).IsRequired().HasMaxLength(320);
            b.Property(u => u.PasswordHash).IsRequired();
            b.Property(u => u.CreatedAt).IsRequired();
        });
    }
}
