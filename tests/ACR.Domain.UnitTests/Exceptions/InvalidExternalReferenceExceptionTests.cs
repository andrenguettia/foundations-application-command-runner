using FluentAssertions;
using ACR.Domain.Exceptions;

namespace ACR.Domain.UnitTests.Exceptions;

public class InvalidExternalReferenceExceptionTests
{
    [Fact]
    public void ExternalReference_WithNonEmptyReference_SetsValue()
    {
        const string EXPECTED_REFERENCE = "SampleReference";
        const string EXPECTED_ERROR_MESSAGE = "Invalid external reference 'SampleReference' specified.";

        var exception = new InvalidExternalReferenceException(EXPECTED_REFERENCE);

        exception.ExternalReference.Should().Be(EXPECTED_REFERENCE);
        exception.Message.Should().Be(EXPECTED_ERROR_MESSAGE);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ExternalReference_WithEmptyReference_SetsValue(string emptyReference)
    {
        const string EXPECTED_REFERENCE = "undefined";
        const string EXPECTED_ERROR_MESSAGE = "Invalid external reference specified.";

        var exception = new InvalidExternalReferenceException(emptyReference);

        exception.ExternalReference.Should().Be(EXPECTED_REFERENCE);
        exception.Message.Should().Be(EXPECTED_ERROR_MESSAGE);
    }
}