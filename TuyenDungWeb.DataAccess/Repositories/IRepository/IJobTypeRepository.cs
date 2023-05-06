using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface IJobTypeRepository : IRepository<JobType>
    {
        void Update(JobType obj);
        public JobType GetById(int id);
        void Save();
    }
}
