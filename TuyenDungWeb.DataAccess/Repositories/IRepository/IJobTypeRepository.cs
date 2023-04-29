using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface IJobTypeRepository : IRepository<JobType>
    {
        void Update(JobType obj);
        void Save();
    }
}
