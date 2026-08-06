namespace ACR.Domain.Exceptions;

public sealed class InvalidExternalReferenceException : OrderDomainException
{
    public string ExternalReference { get; }

    public InvalidExternalReferenceException(string reference) : 
        base($"Invalid external reference {(string.IsNullOrWhiteSpace(reference) ? "" : $"'{reference}' ")}specified.")
    {
        ExternalReference = string.IsNullOrWhiteSpace(reference) ? "undefined" : reference;
    }
}