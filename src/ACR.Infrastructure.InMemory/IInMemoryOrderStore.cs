using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ACR.Domain;

namespace ACR.Infrastructure.InMemory;

interface IInMemoryOrderStore
{
    Order? GetById(Guid id);
    IReadOnlyList<Order> GetAllOrders();
    void Stage(Order order);
    Task PushStagedOrdersAsync();
}