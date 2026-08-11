using FluentAssertions;
using ACR.Domain.Exceptions;
using System;

namespace ACR.Domain.UnitTests;

public class ExternalReferenceTests()
{
    [Theory]
    [InlineData("LOREMIP-SUMDOLORSITAME-TCONSECTETURA")] // an arbitrary length between 30 and 40 characters
    [InlineData("LOREMIP-SUMDOLORSITAME-TCONSEC")] // exactly 30 characters: the minimum length allowed
    [InlineData("LOREMIP-SUMDOLORSITAME-TCONSECTETURADIPI")] // exactly 40 characters: the maximum length allowed
    public void Create_WithMatchingPattern_ReturnsValue(string matchingValue)
    {
        var externalReference = ExternalReference.Create(matchingValue);
        externalReference.Value.Should().Be(matchingValue);
    }

    [Theory]
    [InlineData("lorem1")]
    [InlineData("1lorem")]
    [InlineData("lorem123")]
    [InlineData(" LOREMIP-SUMDOLORSITAME-TCONSECTETURA")]
    [InlineData("LOREMIP-SUMDOLORSITAME-TCONSECTETURA ")]
    [InlineData(" LOREMIP-SUMDOLORSITAME-TCONSECTETURA ")]
    [InlineData("LOREMIP SUMDOLORSITAME-TCONSECTETURA")]
    [InlineData("LO")] // exactly 2 characters: under the minimum allowed
    [InlineData("LOREMIP-SUMDOLORSITAME-TCONSECTETURADIPIR")] // exactly 41 characters: over the maximum allowed
    public void Create_ValueDoesNotMatchPattern_ThrowsInvalidExternalReferenceException(string invalidValue)
    {
        var result = () => ExternalReference.Create(invalidValue);

        result.Should().Throw<InvalidExternalReferenceException>()
        .Which.ExternalReference.Should().Be(invalidValue);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithEmptyInput_ThrowsArgumentException(string emptyValue)
    {
        var result = () => ExternalReference.Create(emptyValue);
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithEmptyInput_ThrowsArgumentNullException()
    {
        var result = () => ExternalReference.Create(externalReference: null);
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Equality_WithMatchingValues_ReturnsTrue()
    {
        const string EXTERNAL_REFERENCE = "LOREMIP-SUMDOLORSITAME-TCONSECTETURA";

        var externalReferenceA = ExternalReference.Create(EXTERNAL_REFERENCE);
        var externalReferenceB = ExternalReference.Create(EXTERNAL_REFERENCE);

        externalReferenceA.Should().Be(externalReferenceB);
    }
}