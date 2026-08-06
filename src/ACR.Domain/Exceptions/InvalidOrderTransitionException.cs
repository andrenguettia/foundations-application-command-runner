namespace ACR.Domain.Exceptions;

public sealed class InvalidOrderTransitionException : OrderDomainException
{
    public OrderStatus CurrentStatus { get; }
    public OrderStatus TargetStatus { get; }

    public InvalidOrderTransitionException(OrderStatus currentStatus, OrderStatus targetStatus):
        base($"Unable to change status '{currentStatus}' to '{targetStatus}'.")
    {
        CurrentStatus = currentStatus;
        TargetStatus = targetStatus;
    }
}