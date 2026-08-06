using System.Collections.Generic;

namespace ACR.Domain;

public interface IOrderTransitionRules
{
    bool IsAllowed(OrderStatus current, OrderStatus target);
    IReadOnlyCollection<OrderStatus> GetAllowedTargets(OrderStatus current);
    IReadOnlyCollection<(OrderStatus From, OrderStatus? To)> EnumerableAll();
}