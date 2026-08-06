namespace ACR.Domain.Exceptions;

public sealed class NegativeCurrencyAmountException : OrderDomainException
{
    public decimal Amount { get; }

    public NegativeCurrencyAmountException(decimal amount) :
        base($"Expected an amount greater than or equal to zero. Found {amount}.")
    {
        Amount = amount;
    }
}