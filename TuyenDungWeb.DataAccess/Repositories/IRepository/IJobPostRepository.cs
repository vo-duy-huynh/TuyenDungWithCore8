using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface IJobPostRepository : IRepository<JobPost>
    {
        public JobPost GetById(int? id);
        public JobPost FirstOrDefault(int? id);
        void Update(JobPost obj);
        int? GetNextId();
    }
}
