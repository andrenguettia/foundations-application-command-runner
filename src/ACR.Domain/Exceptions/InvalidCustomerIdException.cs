namespace ACR.Domain.Exceptions;

public sealed class InvalidCustomerIdException : OrderDomainException
{
    public string CustomerId {get;}

    public InvalidCustomerIdException(string customerId) : 
        base($"Invalid customer Id {(string.IsNullOrWhiteSpace(customerId) ? "" : $"'{customerId}' ")}specified.")
    {
        CustomerId = string.IsNullOrWhiteSpace(customerId) ? "undefined" : customerId;
    }
}