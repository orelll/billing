using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Billing.Application.Models;
using Billing.Application.Services;
using Billing.Application.Validators;
using Billing.Infrastructure.Sql;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace Billing.Application.Tests.Builders;

public class BillingServiceBuilder
{
    private readonly IFixture _fixture;
    public IOrdersContext DbContext { get; private set; }
    public IValidator<OrderModel> OrderValidator{ get; private set; }
    public IPaymentGateway PaymentGateway{ get; private set; }

    public BillingServiceBuilder()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());

        var options = new DbContextOptionsBuilder<OrdersDbContext>()
            .UseInMemoryDatabase(nameof(BillingServiceBuilder))
            .Options;
        DbContext = new OrdersDbContext(options);
        
        OrderValidator = _fixture.Create<IValidator<OrderModel>>();
        OrderValidator.ValidateAsync(Arg.Any<OrderModel>())
            .Returns(ValidationResult.WithSuccess());
        
        PaymentGateway = _fixture.Create<IPaymentGateway>();
        PaymentGateway.ProcessPayment(Arg.Any<PaymentModel>(), Arg.Any<CancellationToken>())
            .Returns((true, Guid.NewGuid()));
    }

    public BillingServiceBuilder WithDbContext(IOrdersContext dbContext)
    {
        DbContext = dbContext;
        return this;
    }
    
    public BillingServiceBuilder WithValidator(IValidator<OrderModel> validator)
    {
        OrderValidator = validator;
        return this;
    }
    public BillingServiceBuilder WithPaymentGateway(IPaymentGateway gateway)
    {
        PaymentGateway = gateway;
        return this;
    }

    public BillingService Build()
    {
        return new BillingService(PaymentGateway, OrderValidator, DbContext);
    }
}