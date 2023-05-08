using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface IJobRepository : IRepository<Job>
    {
        public Job GetById(int id);
        void Update(Job obj);
    }
}
