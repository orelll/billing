using Billing.Core.Entities;
using Billing.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Billing.Infrastructure.Sql;

public class OrdersDbContext : DbContext, IOrdersContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Receipt> Receipts { get; set; }

    public OrdersDbContext(DbContextOptions options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Order>()
            .HasOne<Receipt>(x => x.Receipt)
            .WithOne(x => x.Order)
            .HasForeignKey<Receipt>(x => x.Id);

    }
}