using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface IJobPostTempRepository : IRepository<JobPostTemp>
    {
        List<JobPostTemp> GetAllNoApprove();
        public JobPostTemp GetById(int? id);
        int Count();
        void Update(JobPostTemp obj);
    }
}
