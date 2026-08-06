using System.Threading;
using System.Threading.Tasks;

namespace ACR.Domain;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
}