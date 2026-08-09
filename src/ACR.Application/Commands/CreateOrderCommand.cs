namespace ACR.Application.Commands;

public sealed record CreateOrderCommand(
    string CustomerId,
    decimal TotalAmount,
    string CurrencyCode,
    string ExternalReference);
