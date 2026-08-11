using FluentAssertions;
using ACR.Domain.Exceptions;
using System;

namespace ACR.Domain.UnitTests;

public class CustomerIdTests()
{
    [Theory]
    [InlineData("ABCDEFGHIJ")] // an arbitrary length between 8 and 20 characters
    [InlineData("ABCDEFGH")] // exactly 8 characters: the minimum length allowed
    [InlineData("ABCDEFGHIJKLMNOPQRST")] // exactly 20 characters: the maximum length allowed
    public void Create_WithMatchingPattern_ReturnsValue(string matchingValue)
    {
        var customerId = CustomerId.Create(matchingValue);
        customerId.Value.Should().Be(matchingValue);
    }

    [Theory]
    [InlineData("A1")]
    [InlineData("1A")]
    [InlineData("abc123")]
    [InlineData(" abc123")]
    [InlineData("abc123 ")]
    [InlineData(" abc123 ")]
    [InlineData("A 123")]
    [InlineData("A-123")]
    [InlineData("ABCDEFG")] // exactly 7 characters: under the minimum allowed
    [InlineData("ABCDEFGHIJKLMNOPQRSTU")] // exactly 21 characters: over the maximum allowed
    public void Create_ValueDoesNotMatchPattern_ThrowsInvalidCustomerIdException(string invalidValue)
    {
        var result = () => CustomerId.Create(invalidValue);

        result.Should().Throw<InvalidCustomerIdException>()
        .Which.CustomerId.Should().Be(invalidValue);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithEmptyInput_ThrowsArgumentException(string emptyValue)
    {
        var result = () => CustomerId.Create(emptyValue);
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithEmptyInput_ThrowsArgumentNullException()
    {
        var result = () => CustomerId.Create(customerId: null);
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Equality_WithMatchingValues_ReturnsTrue()
    {
        const string CUSTOMER_ID = "ABCDEFGHIJ";

        var customerIdA = CustomerId.Create(CUSTOMER_ID);
        var customerIdB = CustomerId.Create(CUSTOMER_ID);

        customerIdA.Should().Be(customerIdB);
    }
}