using TuyenDungWeb.Model;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface IJobTypeRepository : IRepository<JobType>
    {
        void Update(JobType obj);
        void Save();
    }
}
