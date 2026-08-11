using FluentAssertions;
using ACR.Domain.Exceptions;

namespace ACR.Domain.UnitTests.Exceptions;

public class NegativeCurrencyAmountExceptionTests()
{
    [Fact]
    public void Amount_WithNegativeValue_SetsValue()
    {
        const decimal EXPECTED_NEGATIVE_AMOUNT = -5;
        const string EXPECTED_ERROR_MESSAGE = "Expected an amount greater than or equal to zero. Found -5.";

        var exception = new NegativeCurrencyAmountException(EXPECTED_NEGATIVE_AMOUNT);

        exception.Amount.Should().Be(EXPECTED_NEGATIVE_AMOUNT);
        exception.Message.Should().Be(EXPECTED_ERROR_MESSAGE);
    }
}