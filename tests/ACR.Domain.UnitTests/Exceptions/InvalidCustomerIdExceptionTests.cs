using FluentAssertions;
using ACR.Domain.Exceptions;

namespace ACR.Domain.UnitTests.Exceptions;

public class InvalidCustomerIdExceptionTests
{
    [Fact]
    public void CustomerId_WithNonEmptyId_SetsValue()
    {
        const string EXPECTED_CUSTOMER_ID = "abc123";
        const string EXPECTED_ERROR_MESSAGE = "Invalid customer Id 'abc123' specified.";

        var exception = new InvalidCustomerIdException(EXPECTED_CUSTOMER_ID);

        exception.CustomerId.Should().Be(EXPECTED_CUSTOMER_ID);
        exception.Message.Should().Be(EXPECTED_ERROR_MESSAGE);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void CustomerId_WithEmptyId_SetsValue(string emptyId)
    {
        const string EXPECTED_CUSTOMER_ID = "undefined";
        const string EXPECTED_ERROR_MESSAGE = "Invalid customer Id specified.";

        var exception = new InvalidCustomerIdException(emptyId);

        exception.CustomerId.Should().Be(EXPECTED_CUSTOMER_ID);
        exception.Message.Should().Be(EXPECTED_ERROR_MESSAGE);
    }
}