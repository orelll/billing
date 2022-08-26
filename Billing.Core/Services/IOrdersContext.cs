using Billing.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Billing.Core.Services;

public interface IOrdersContext
{
    DbSet<Order> Orders { get; set; }
    DbSet<Receipt> Receipts { get; set; }
}