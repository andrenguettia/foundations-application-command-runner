using System;
using System.Threading;
using System.Threading.Tasks;

namespace ACR.Domain;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Order?> GetByExternalReferenceAsync(ExternalReference externalReference, CancellationToken cancellationToken);
    Task StageAsync(Order order, CancellationToken cancellationToken);
}