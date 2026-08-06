using System;
using System.Text.RegularExpressions;
using ACR.Domain.Exceptions;

namespace ACR.Domain;

public partial record ExternalReference
{
    /// <summary>
    /// Assume the business requirements defined an external reference as:
    /// - A string 30-40 characters in length
    /// - Containing only uppercased letters, digits and hyphens
    /// </summary>
    [GeneratedRegex("^[A-Z0-9-]{30,40}$")]
    private static partial Regex ExternalReferencePattern();

    public string Value { get; }

    private ExternalReference(string value)
    {
        Value = value;
    }

    public static ExternalReference Create(string externalReference)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(externalReference);
        if(!ExternalReferencePattern().IsMatch(externalReference))
        {
            throw new InvalidExternalReferenceException(externalReference);
        }

        return new ExternalReference(externalReference);
    }

    public override string ToString() => Value;
}