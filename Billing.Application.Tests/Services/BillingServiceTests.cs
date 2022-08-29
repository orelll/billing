using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Billing.Application.Models;
using Billing.Application.Services;
using Billing.Application.Tests.Builders;
using Billing.Application.Validators;
using FluentAssertions;
using NSubstitute;

namespace Billing.Application.Tests.Services;

public class BillingServiceTests
{
    private readonly IFixture _fixture;

    public BillingServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
    }
    
    [Fact]
    public async Task When_ValidateOrderIsCalled_Then_ValidatorIsCalled()
    {
        //a 
        var validatorSpy = _fixture.Create<IValidator<OrderModel>>();
        var sut = new BillingServiceBuilder()
            .WithValidator(validatorSpy)
            .Build();

        var requestDummy = _fixture.Create<OrderModel>();

        //aa
        await sut.ValidateOrder(requestDummy, CancellationToken.None);

        //aaa
        await validatorSpy.Received().ValidateAsync(Arg.Any<OrderModel>());
    } 
    
    [Fact]
    public async Task When_ValidateOrderIsCalled_Then_ModelIsPassed()
    {
        //a 
        var validatorSpy = _fixture.Create<IValidator<OrderModel>>();
        var sut = new BillingServiceBuilder()
            .WithValidator(validatorSpy)
            .Build();

        var requestDummy = _fixture.Create<OrderModel>();

        //aa
        await sut.ValidateOrder(requestDummy, CancellationToken.None);

        //aaa
        await validatorSpy.Received().ValidateAsync(requestDummy);
    } 
    
    [Fact]
    public async Task When_CreateOrderIsCalled_Then_ValidatorIsCalled()
    {
        //a 
        var validatorSpy = _fixture.Create<IValidator<OrderModel>>();
        validatorSpy.ValidateAsync(Arg.Any<OrderModel>())
            .Returns(Task.FromResult(ValidationResult.WithSuccess()));
        var sut = new BillingServiceBuilder()
            .WithValidator(validatorSpy)
            .Build();

        var requestDummy = _fixture.Create<OrderModel>();

        //aa
        await sut.CreateOrder(requestDummy, CancellationToken.None);

        //aaa
        await validatorSpy.Received().ValidateAsync(requestDummy);
    }
    
    [Fact]
    public async Task When_CreateOrderIsCalled_WithInproperInput_Then_ErrorsResponseIsReturned()
    {
        //a 
        var validationMsg = Guid.NewGuid().ToString();
        var validatorStub = _fixture.Create<IValidator<OrderModel>>();
        validatorStub.ValidateAsync(Arg.Any<OrderModel>())
            .Returns(ValidationResult.WithErrors(validationMsg));
        
        var sut = new BillingServiceBuilder()
            .WithValidator(validatorStub)
            .Build();

        var requestDummy = _fixture.Create<OrderModel>();

        //aa
        var response = await sut.CreateOrder(requestDummy, CancellationToken.None);

        //aaa
        response.Succeed.Should().BeFalse();
        response.Errors.Length.Should().Be(1);
        response.Errors.First().Should().Be(validationMsg);
    }
    
    [Fact]
    public async Task When_CreateOrderIsCalled_Then_PaymentGatewayIsCalled()
    {
        //a
        var gatewaySpy = _fixture.Create<IPaymentGateway>();
        var sut = new BillingServiceBuilder()
            .WithPaymentGateway(gatewaySpy)
            .Build();

        var requestDummy = _fixture.Create<OrderModel>();

        //aa
        await sut.CreateOrder(requestDummy, CancellationToken.None);

        //aaa
        await gatewaySpy.Received().ProcessPayment(Arg.Any<PaymentModel>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task When_CreateOrderIsCalled_Then_PaymentDataIsPassedToGateway()
    {
        //a
        var gatewaySpy = _fixture.Create<IPaymentGateway>();
        var sut = new BillingServiceBuilder()
            .WithPaymentGateway(gatewaySpy)
            .Build();

        var requestDummy = _fixture.Create<OrderModel>();

        //aa
        await sut.CreateOrder(requestDummy, CancellationToken.None);

        //aaa
        await gatewaySpy.Received().ProcessPayment(Arg.Is<PaymentModel>(pay =>
        pay.Currency == requestDummy.Currency &&
        pay.DecimalPart == requestDummy.DecimalPart && 
        pay.FullPart == requestDummy.FullPart &&
        pay.GatewayId == requestDummy.GatewayId
        ), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task When_CreateOrderIsCalled_Then_CancellationTokenIsPopulatedToGateway()
    {
        //a
        var gatewaySpy = _fixture.Create<IPaymentGateway>();
        var sut = new BillingServiceBuilder()
            .WithPaymentGateway(gatewaySpy)
            .Build();

        var requestDummy = _fixture.Create<OrderModel>();
        var cancellationToken = new CancellationToken();
        
        //aa
        await sut.CreateOrder(requestDummy, cancellationToken);

        //aaa
        await gatewaySpy.Received().ProcessPayment(Arg.Any<PaymentModel>(), cancellationToken);
    }
    
    [Fact]
    public async Task When_CreateOrderIsCalled_WithGatewayNegativeResponse_Then_ErrorsResponseIsReturned()
    {
        //a
        var gatewaySpy = _fixture.Create<IPaymentGateway>();
        gatewaySpy.ProcessPayment(Arg.Any<PaymentModel>(), Arg.Any<CancellationToken>())
            .Returns((false, Guid.Empty));
        var sut = new BillingServiceBuilder()
            .WithPaymentGateway(gatewaySpy)
            .Build();

        var requestDummy = _fixture.Create<OrderModel>();
        var cancellationToken = new CancellationToken();
        
        //aa
        var response = await sut.CreateOrder(requestDummy, cancellationToken);

        //aaa
        response.Succeed.Should().BeFalse();
        response.Errors.Length.Should().Be(1);
        response.Errors.First().Should().Be("Payment gateway returned error");
    }
    
    [Fact]
    public async Task When_CreateOrderIsCalled_WithGatewaySuccess_Then_DataIsPersisted()
    {
        //a
        var requestDummy = _fixture.Create<OrderModel>();
        var gatewayPaymentIdDummy = Guid.NewGuid();
        var gatewayStub = _fixture.Create<IPaymentGateway>();
        gatewayStub.ProcessPayment(Arg.Any<PaymentModel>(), Arg.Any<CancellationToken>())
            .Returns((true, gatewayPaymentIdDummy));

        var builder = new BillingServiceBuilder()
            .WithPaymentGateway(gatewayStub);
        var sut = builder.Build();

        //aa
        var response = await sut.CreateOrder(requestDummy, CancellationToken.None);

        //aaa
        var persistedOrder = builder.DbContext.Orders.FirstOrDefault(x => x.Id == response!.Result.OrderId);
        persistedOrder.Description.Should().Be(requestDummy.Description);
        persistedOrder.UserId.Should().Be(requestDummy.UserId);
        persistedOrder.OrderNumber.Should().Be(requestDummy.OrderNumber);
        persistedOrder.Payment.Currency.Should().Be(requestDummy.Currency);
        persistedOrder.Payment.DecimalPart.Should().Be(requestDummy.DecimalPart);
        persistedOrder.Payment.FullPart.Should().Be(requestDummy.FullPart);
        persistedOrder.Payment.GatewayId.Should().Be(requestDummy.GatewayId);
        persistedOrder.Payment.GatewayPaymentId.Should().Be(gatewayPaymentIdDummy);
    }
    
    [Fact]
    public async Task When_CreateOrderIsCalled_Then_FullReceiptDataIsReturned()
    {
        //a
        var builder = new BillingServiceBuilder();
        var requestDummy = _fixture.Create<OrderModel>();
        var gatewayPaymentIdDummy = Guid.NewGuid();
        var gatewayStub = builder.PaymentGateway;
        gatewayStub.ProcessPayment(Arg.Any<PaymentModel>(), Arg.Any<CancellationToken>())
            .Returns((true, gatewayPaymentIdDummy));

        
        var sut = builder.Build();

        //aa
        var response = await sut.CreateOrder(requestDummy, CancellationToken.None);

        //aaa
        response.Result.Should().NotBeNull();
        response.Succeed.Should().BeTrue();
        response.Result.Description.Should().Be(requestDummy.Description);
        response.Result.UserId.Should().Be(requestDummy.UserId);
        response.Result.PaymentAmount.Should().Be( $"{requestDummy.FullPart}.{requestDummy.DecimalPart} {requestDummy.Currency}");
        response.Result.PaymentGatewayId.Should().Be(requestDummy.GatewayId);
    }
    
}