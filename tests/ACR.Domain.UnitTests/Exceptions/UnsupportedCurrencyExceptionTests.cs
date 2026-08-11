using FluentAssertions;
using ACR.Domain.Exceptions;

namespace ACR.Domain.UnitTests.Exceptions;

public class UnsupportedCurrencyExceptionTests()
{
    [Fact]
    public void CurrencyCode_WithNonEmptyCode_SetsValue()
    {
        const string EXPECTED_CURRENCY_CODE = "abc";
        const string EXPECTED_ERROR_MESSAGE = "Currency 'abc' not found.";

        var exception = new UnsupportedCurrencyException(EXPECTED_CURRENCY_CODE);

        exception.CurrencyCode.Should().Be(EXPECTED_CURRENCY_CODE);
        exception.Message.Should().Be(EXPECTED_ERROR_MESSAGE);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void CurrencyCode_WithEmptyCode_SetsValue(string emptyCode)
    {
        const string EXPECTED_CURRENCY_CODE = "undefined";
        const string EXPECTED_ERROR_MESSAGE = "Currency not found.";

        var exception = new UnsupportedCurrencyException(emptyCode);

        exception.CurrencyCode.Should().Be(EXPECTED_CURRENCY_CODE);
        exception.Message.Should().Be(EXPECTED_ERROR_MESSAGE);
    }
}