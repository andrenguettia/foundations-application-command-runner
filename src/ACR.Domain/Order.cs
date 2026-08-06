using System;
using ACR.Domain.Exceptions;

namespace ACR.Domain;

public sealed class Order
{
    private static IOrderTransitionRules _OrderTransitionRules;

    public Guid Id { get; }
    public CustomerId CustomerId { get; }
    public OrderStatus Status { get; }
    public Currency TotalAmount { get; }
    public ExternalReference? ExternalReference { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }

    private Order(
        Guid id,
        CustomerId customerId,
        OrderStatus status,
        Currency totalAmount,
        ExternalReference? externalReference,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        CustomerId = customerId;
        Status = status;
        TotalAmount = totalAmount;
        ExternalReference = externalReference;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Order Create(
        CustomerId customerId,
        Currency currency,
        DateTime currentDateTime,
        ExternalReference? externalReference = null)
    {
        return Create(customerId, currency, currentDateTime, externalReference, new OrderTransitionRules());
    }

    public static Order Create(
        CustomerId customerId,
        Currency currency,
        DateTime currentDateTime,
        ExternalReference? externalReference,
        IOrderTransitionRules orderTransitionRules)
    {
        _OrderTransitionRules = orderTransitionRules;

        return new Order(
            Guid.NewGuid(),
            customerId,
            OrderStatus.Pending,
            currency,
            externalReference,
            currentDateTime,
            currentDateTime);
    }

    public static Order Reconstitute(
        Guid id,
        CustomerId customerId,
        OrderStatus orderStatus,
        Currency totalAmount,
        ExternalReference? externalReference,
        DateTime createdAt,
        DateTime updatedAt
    )
    {
        return new Order(id, customerId, orderStatus, totalAmount, externalReference, createdAt, updatedAt);
    }

    public Order TransitionTo(OrderStatus targetStatus, DateTime currentDateTime)
    {
        if(!_OrderTransitionRules.IsAllowed(Status, targetStatus))
        {
            throw new InvalidOrderTransitionException(Status, targetStatus);
        }

        return new Order(Id, CustomerId, targetStatus, TotalAmount, ExternalReference, CreatedAt, currentDateTime);
    }
}