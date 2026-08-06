namespace ACR.Domain.Exceptions;

public sealed class UnsupportedCurrencyException : OrderDomainException
{
    public string CurrencyCode { get; }

    public UnsupportedCurrencyException(string currencyCode) :
        base($"Currency {(string.IsNullOrWhiteSpace(currencyCode) ? "" : $"'{currencyCode}' ")}not found.")
    {
        CurrencyCode = string.IsNullOrWhiteSpace(currencyCode) ? "undefined" : currencyCode;
    }
}