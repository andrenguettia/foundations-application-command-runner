using FluentAssertions;
using ACR.Domain;
using ACR.Domain.Exceptions;

namespace ACR.Domain.UnitTests;

public class InvalidOrderTransitionExceptionTests
{
    [Fact]
    public void OrderStatus_WithOrders_SetsValues()
    {
        const OrderStatus EXPECTED_CURRENT_STATUS = OrderStatus.Confirmed;
        const OrderStatus EXPECTED_TARGET_STATUS = OrderStatus.Cancelled;
        const string EXPECTED_ERROR_MESSAGE = "Unable to change status 'Confirmed' to 'Cancelled'.";

        var exception = new InvalidOrderTransitionException(EXPECTED_CURRENT_STATUS, EXPECTED_TARGET_STATUS);

        exception.CurrentStatus.Should().Be(EXPECTED_CURRENT_STATUS);
        exception.TargetStatus.Should().Be(EXPECTED_TARGET_STATUS);
        exception.Message.Should().Be(EXPECTED_ERROR_MESSAGE);
    }
}