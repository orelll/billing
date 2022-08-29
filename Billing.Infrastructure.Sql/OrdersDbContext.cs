using Billing.Application.Entities;
using Billing.Application.Services;
using Microsoft.EntityFrameworkCore;

namespace Billing.Infrastructure.Sql;

public class OrdersDbContext : DbContext, IOrdersContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Receipt> Receipts { get; set; }
    public DbSet<Payment> Payments { get; set; }

    public OrdersDbContext(DbContextOptions options)
        : base(options)
    {
    }
    
    public async Task SaveChangesAsync(CancellationToken token)
    {
        await base.SaveChangesAsync(token);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Order>()
            .Property<int>(nameof(Order.Id))
            .UseIdentityColumn();
        
        modelBuilder.Entity<Receipt>()
            .Property<int>(nameof(Receipt.Id))
            .UseIdentityColumn();  
        
        modelBuilder.Entity<Payment>()
            .Property<int>(nameof(Payment.Id))
            .UseIdentityColumn();
        
        modelBuilder.Entity<Order>()
            .HasOne<Receipt>(x => x.Receipt)
            .WithOne(x => x.Order)
            .HasForeignKey<Receipt>(x => x.OrderId);    
        
        modelBuilder.Entity<Order>()
            .HasOne<Payment>(x => x.Payment)
            .WithOne(x => x.Order)
            .HasForeignKey<Payment>(x => x.OrderId);

    }
}