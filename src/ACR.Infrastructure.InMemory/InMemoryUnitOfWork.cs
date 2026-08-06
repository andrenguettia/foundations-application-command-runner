using System.Threading;
using System.Threading.Tasks;
using ACR.Domain;

namespace ACR.Infrastructure.InMemory;

public sealed class InMemoryUnitOfWork : IUnitOfWork
{
    private readonly InMemoryOrderStore _orderStore;

    public InMemoryUnitOfWork(InMemoryOrderStore orderStore)
    {
        _orderStore = orderStore;    
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await _orderStore.PushStagedOrdersAsync();
    }
}