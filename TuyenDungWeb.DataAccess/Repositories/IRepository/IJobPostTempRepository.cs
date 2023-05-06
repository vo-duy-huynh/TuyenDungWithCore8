using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface IJobPostTempRepository : IRepository<JobPostTemp>
    {
        void Update(JobPostTemp obj);
    }
}
