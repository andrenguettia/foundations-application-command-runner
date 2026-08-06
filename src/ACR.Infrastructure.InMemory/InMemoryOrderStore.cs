using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACR.Domain;

namespace ACR.Infrastructure.InMemory;

public sealed class InMemoryOrderStore : IInMemoryOrderStore
{
    private readonly Dictionary<Guid, Order> _savedOrders = new();
    private readonly List<Order> _stagedOrders = new();

    public Order? GetById(Guid id) => _savedOrders.TryGetValue(id, out var order) ? order : null;

    public IReadOnlyList<Order> GetAllOrders() => _savedOrders.Values.OrderBy(order => order.CreatedAt).ToList();

    public void Stage(Order order) => _stagedOrders.Add(order);

    public Task PushStagedOrdersAsync()
    {
        foreach (var order in _stagedOrders)
        {
            _savedOrders[order.Id] = order;
        }

        _stagedOrders.Clear();

        return Task.CompletedTask;
    }
}