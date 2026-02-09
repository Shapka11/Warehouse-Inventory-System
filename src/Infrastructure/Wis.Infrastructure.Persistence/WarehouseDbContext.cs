using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class WarehouseDbContext : DbContext
{
    public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options) { }

    public DbSet<RollEntity> Rolls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RollEntity>()
            .Property(r => r.AddedDate)
            .HasColumnType("timestamp with time zone");

        modelBuilder.Entity<RollEntity>()
            .Property(r => r.RemovedDate)
            .HasColumnType("timestamp with time zone");

        modelBuilder.Entity<RollEntity>()
            .HasIndex(r => r.AddedDate);

        modelBuilder.Entity<RollEntity>()
            .HasIndex(r => r.RemovedDate);
    }
}