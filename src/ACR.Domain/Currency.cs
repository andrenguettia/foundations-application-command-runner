using System;
using System.Collections.Generic;
using System.Linq;
using ACR.Domain.Exceptions;

namespace ACR.Domain;

public sealed record Currency
{
    private static IReadOnlyCollection<string> SupportedCurrencyCodes =
    [
        "USD", "EUR", "GBP", "CAD", "AUD", "JPY", "CHF"
    ];

    public decimal Amount { get; }
    public string CurrencyCode { get; }
    
    private Currency(decimal amount, string currencyCode)
    {
        Amount = amount;
        CurrencyCode = currencyCode;
    }

    public static Currency Create(decimal amount, string currencyCode)
    {
        if(amount < 0m)
        {
            throw new NegativeCurrencyAmountException(amount);
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(currencyCode);

        currencyCode = currencyCode.Trim().ToUpperInvariant();
        if(!SupportedCurrencyCodes.Contains(currencyCode))
        {
            throw new UnsupportedCurrencyException(currencyCode);
        }

        return new Currency(amount, currencyCode);
    }
}