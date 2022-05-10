using System.Threading.Tasks;

namespace Nxt.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : class;
        Task<int> Commit();
        void Rollback();
     }
}
