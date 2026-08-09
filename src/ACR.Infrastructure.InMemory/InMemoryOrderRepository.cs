using System;
using System.Threading;
using System.Threading.Tasks;
using ACR.Domain;

namespace ACR.Infrastructure.InMemory;

public sealed class InMemoryOrderRepository : IOrderRepository
{
    private readonly InMemoryOrderStore _orderStore;

    public InMemoryOrderRepository(InMemoryOrderStore orderStore)
    {
        _orderStore = orderStore;
    }

    public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_orderStore.GetById(id));
    }

    public Task<Order?> GetByExternalReferenceAsync(ExternalReference externalReference, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_orderStore.GetByExternalReference(externalReference));
    }

    public Task StageAsync(Order order, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _orderStore.Stage(order);
        return Task.CompletedTask;
    }
}