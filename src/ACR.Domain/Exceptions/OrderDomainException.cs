using System;

namespace ACR.Domain.Exceptions;

public abstract class OrderDomainException : Exception
{

    protected OrderDomainException(string message) : base(message)
    {
    }
}