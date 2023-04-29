using TuyenDungWeb.Model;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface IJobPostRepository : IRepository<JobPost>
    {
        void Update(JobPost obj);
    }
}
