using System;
using System.Text.RegularExpressions;
using ACR.Domain.Exceptions;

namespace ACR.Domain;

public partial record CustomerId
{
    /// <summary>
    /// Assume the business requirements defined a customer ID as:
    /// - A string 8-20 characters in length
    /// - Containing only uppercased letters and digits
    /// </summary>
    [GeneratedRegex("^[A-Z][A-Z0-9]{7,19}$")]
    private static partial Regex CustomerIdPattern();

    public string Value { get; }

    private CustomerId(string value)
    {
        Value = value;
    }

    public static CustomerId Create(string customerId)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(customerId);
        if(!CustomerIdPattern().IsMatch(customerId))
        {
            throw new InvalidCustomerIdException(customerId);
        }

        return new CustomerId(customerId);
    }

    public override string ToString() => Value;
}