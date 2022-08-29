using Billing.Application.Models;
using Billing.Application.Services;
using Billing.Application.Validators;
using Billing.Infrastructure.Sql;

namespace Billing.Api.ExtensionMethods;

public static class IServiceCollectionExtensionMethods
{
    public static IServiceCollection RegisterAppServices(this IServiceCollection services)
    {
        services.AddScoped<IBillingService, BillingService>();
        services.AddScoped<IPaymentGateway, PaymentGateway>();
        services.AddScoped<IValidator<OrderModel>, OrderValidator>();
        services.AddScoped<IOrdersContext, OrdersDbContext>();

        return services;
    }
}