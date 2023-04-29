using TuyenDungWeb.Model;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface IJobRepository : IRepository<Job>
    {
        void Update(Job obj);
    }
}
