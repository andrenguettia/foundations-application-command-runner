using FluentAssertions;
using Moq;
using ACR.Domain.Exceptions;
using System;

namespace ACR.Domain.UnitTests;

public class OrderTests()
{
    private readonly DateTime _now = new DateTime(2026, 1, 1);        

    [Fact]
    public void Create_WithValidInputs_ReturnsOrder()
    {
        var expectedCustomerId = GetCustomerId();
        var expectedCurrency = GetCurrency();
        var expectedExternalReference = GetExternalReference();
        
        var actualOrder = Order.Create(expectedCustomerId, expectedCurrency, _now, expectedExternalReference);

        actualOrder.Id.Should().NotBeEmpty();
        actualOrder.Status.Should().Be(OrderStatus.Pending);
        actualOrder.CustomerId.Value.Should().Be(expectedCustomerId.Value);
        actualOrder.TotalAmount.Amount.Should().Be(expectedCurrency.Amount);
        actualOrder.TotalAmount.CurrencyCode.Should().Be(expectedCurrency.CurrencyCode);
        actualOrder.ExternalReference.Value.Should().Be(expectedExternalReference.Value);
        actualOrder.CreatedAt.Should().Be(_now);
        actualOrder.UpdatedAt.Should().Be(_now);
    }

    [Fact]
    public void Reconstitute_WithValidInputs_ReturnsOrder()
    {
        var order = Order.Create(GetCustomerId(), GetCurrency(), _now, GetExternalReference());

        var reconstitutedOrder = Order.Reconstitute(order.Id, order.CustomerId, order.Status, order.TotalAmount, order.ExternalReference, order.CreatedAt, order.UpdatedAt);

        reconstitutedOrder.Should().BeEquivalentTo(order);
    }

    [Fact]
    public void UpdateStatus_WithAllowedTransitionRules_UpdatedOrderStatus()
    {
        const OrderStatus EXPECTED_STATUS = OrderStatus.Confirmed;

        var orderTransitionRules = new Mock<IOrderTransitionRules>();
        orderTransitionRules.Setup(x => x.IsAllowed(It.IsAny<OrderStatus>(), It.IsAny<OrderStatus>()))
                            .Returns(true);

        var currentOrder = Order.Create(GetCustomerId(), GetCurrency(), _now, GetExternalReference(), orderTransitionRules.Object);

        var newOrder = currentOrder.UpdateStatus(EXPECTED_STATUS, _now);

        newOrder.Status.Should().Be(EXPECTED_STATUS);
    }

    [Fact]
    public void UpdateStatus_WithDisllowedTransitionRules_ThrowsException()
    {
        var orderTransitionRules = new Mock<IOrderTransitionRules>();
        orderTransitionRules.Setup(x => x.IsAllowed(It.IsAny<OrderStatus>(), It.IsAny<OrderStatus>()))
                            .Returns(false);

        var currentOrder = Order.Create(GetCustomerId(), GetCurrency(), _now, GetExternalReference(), orderTransitionRules.Object);

        var result = () => currentOrder.UpdateStatus(OrderStatus.Confirmed, _now);

        result.Should().Throw<InvalidOrderTransitionException>();
    }

    private CustomerId GetCustomerId()
    {
        return CustomerId.Create("SAMPLECUSTOMERID");
    }

    private Currency GetCurrency()
    {
        const decimal AMOUNT = 123;
        const string CURRENCY_CODE = "USD";
        
        return Currency.Create(AMOUNT, CURRENCY_CODE);
    }

    private ExternalReference GetExternalReference()
    {
        return ExternalReference.Create("SAMPLE-EXTERNAL-REFERENCE-123-456");
    }
}