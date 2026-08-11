using FluentAssertions;
using ACR.Domain.Exceptions;
using System;

namespace ACR.Domain.UnitTests;

public class CurrencyTests()
{
    [Fact]
    public void Create_ValidAmountAndCurrencyCode_ReturnsCurrency()
    {
        const decimal EXPECTED_AMOUNT = 123;
        const string EXPECTED_CURRENCY_CODE = "USD";

        var currency = Currency.Create(EXPECTED_AMOUNT, EXPECTED_CURRENCY_CODE);

        currency.Amount.Should().Be(EXPECTED_AMOUNT);
        currency.CurrencyCode.Should().Be(EXPECTED_CURRENCY_CODE);
    }

    [Fact]
    public void Create_WithNegativeAmount_ThrowsException()
    {
        const decimal NEGATIVE_AMOUNT = -123;
        const string CURRENCY_CODE = "USD";

        var result = () => Currency.Create(NEGATIVE_AMOUNT, CURRENCY_CODE);
        result.Should().Throw<NegativeCurrencyAmountException>();
    }

    [Fact]
    public void Create_WithUnsupportedCurrencyCode_ThrowsException()
    {
        const decimal AMOUNT = 123;
        const string UNSUPPORTED_CURRENCY_CODE = "UNSUPPORTED";

        var result = () => Currency.Create(AMOUNT, UNSUPPORTED_CURRENCY_CODE);
        result.Should().Throw<UnsupportedCurrencyException>();
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("")]
    [InlineData(null)]
    public void Create_WithEmptyCurrencyCode_ThrowsException(string emptyCurrencyCode)
    {
        const decimal AMOUNT = 123;

        var result = () => Currency.Create(AMOUNT, emptyCurrencyCode);
        result.Should().Throw<ArgumentException>();
    }
}