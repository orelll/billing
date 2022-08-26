using Billing.Infrastructure.Sql;
using Microsoft.EntityFrameworkCore;

namespace Billing.Api;

public static class DbMigrator
{
    public static async Task Migrate(WebApplication app)
    {
        using (var scope = ((IApplicationBuilder) app).ApplicationServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetService<OrdersDbContext>();
            dbContext.Database.Migrate();
        }
    }
}