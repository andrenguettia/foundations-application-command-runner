using System;
using System.Collections.Generic;
using System.Linq;

namespace ACR.Domain;

public class OrderTransitionRules : IOrderTransitionRules
{
    private readonly IReadOnlyDictionary<OrderStatus, IReadOnlyList<OrderStatus>> TransitionRules =
    new Dictionary<OrderStatus, IReadOnlyList<OrderStatus>>
    {
        [OrderStatus.Pending] = [OrderStatus.Confirmed, OrderStatus.Cancelled],
        [OrderStatus.Confirmed] = [OrderStatus.Cancelled],
        [OrderStatus.Cancelled] = Array.Empty<OrderStatus>()
    };

    public bool IsAllowed(OrderStatus current, OrderStatus target)
    {
        return TransitionRules[current].Contains(target);
    }

    public IReadOnlyCollection<OrderStatus> GetAllowedTargets(OrderStatus current)
    {
        return TransitionRules[current];
    }

    public IReadOnlyCollection<(OrderStatus From, OrderStatus? To)> EnumerableAll()
    {
        var flattenRules = TransitionRules
                        .SelectMany(fromStatus => 
                            fromStatus.Value
                            .Select(toStatus => (OrderStatus?)toStatus)
                            .DefaultIfEmpty(null)
                            .Select(toStatus => (From: fromStatus.Key, To: toStatus)))
                        .ToList();

        return flattenRules;
    }
}