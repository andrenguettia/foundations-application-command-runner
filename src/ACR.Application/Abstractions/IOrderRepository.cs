using System;
using System.Threading;
using System.Threading.Tasks;
using ACR.Domain;

namespace ACR.Application.Abstractions;

public interface IOrderRepository
{
    Task<Order?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Order?> FindByExternalReferenceAsync(string externalReference, CancellationToken cancellantionToken);

    Task AddAsync(Order order, CancellationToken cancellationToken);

    Task UpdateAsync(Order order, CancellationToken cancellationToken);
}