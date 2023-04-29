using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface IJobRepository : IRepository<Job>
    {
        void Update(Job obj);
    }
}
