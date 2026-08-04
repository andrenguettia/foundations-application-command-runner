using System.Threading;
using System.Threading.Tasks;

namespace ACR.Application.Abstractions;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken);
}