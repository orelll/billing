using Billing.Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace Billing.Application.Services
{
    public interface IOrdersContext
    {
        DbSet<Order> Orders { get; set; }
        DbSet<Receipt> Receipts { get; set; }
        DbSet<Payment> Payments { get; set; }

        Task SaveChangesAsync(CancellationToken token);
    }
}