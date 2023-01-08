using System.Threading.Tasks;

namespace SystemV1.Domain.Core.Interfaces.Uow
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync();
    }
}