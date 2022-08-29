using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Billing.Application.Models;
using Billing.Application.Validators;
using FluentAssertions;

namespace Billing.Application.Tests.Validators;

public class OrderValidatorTests
{
    private readonly IFixture _fixture;

    public OrderValidatorTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
    }
    
    [Fact]
    public async Task When_ValidateOrderIsCalled_With_EmptyOrderNumber_Then_ValidationFails()
    {
        //a 
        var sut = new OrderValidator();

        var requestDummy = _fixture.Build<OrderModel>()
            .With(x => x.OrderNumber, Guid.Empty)
            .Create();

        //aa
        var validationResult = await sut.ValidateAsync(requestDummy);

        //aaa
        validationResult.Ok.Should().BeFalse();
        validationResult.Errors.Length.Should().Be(1);
        validationResult.Errors.First().Should().Be("OrderNumber contains empty data");
    }
    
    [Fact]
    public async Task When_ValidateOrderIsCalled_With_WrongData_Then_ResponseIsNotSucceeded()
    {
        //a 
        var sut = new OrderValidator();

        var requestDummy = _fixture.Build<OrderModel>()
            .With(x => x.OrderNumber, Guid.Empty)
            .Create();

        //aa
        var validationResult = await sut.ValidateAsync(requestDummy);

        //aaa
        validationResult.Ok.Should().BeFalse();
    }
    
    [Fact]
    public async Task When_ValidateOrderIsCalled_With_MultipleErrors_Then_AllAreListed()
    {
        //a 
        var sut = new OrderValidator();

        var requestDummy = _fixture.Build<OrderModel>()
            .With(x => x.OrderNumber, Guid.Empty)
            .With(x => x.GatewayId, Guid.Empty)
            .With(x => x.Currency, "abc")
            .Create();

        //aa
        var validationResult = await sut.ValidateAsync(requestDummy);

        //aaa
        validationResult.Ok.Should().BeFalse();
        validationResult.Errors.Length.Should().Be(3);
        validationResult.Errors.Should().Contain("OrderNumber contains empty data");
        validationResult.Errors.Should().Contain("Missing payment service data");
        validationResult.Errors.Should().Contain("Not supported currency");
    }
    
    [Fact]
    public async Task When_ValidateOrderIsCalled_With_NoErrors_Then_SuccessIsReturned()
    {
        //a 
        var sut = new OrderValidator();

        var requestDummy = _fixture.Build<OrderModel>()
            .With(x => x.Currency, "eur")
            .Create();

        //aa
        var validationResult = await sut.ValidateAsync(requestDummy);

        //aaa
        validationResult.Ok.Should().BeTrue();
    }
}