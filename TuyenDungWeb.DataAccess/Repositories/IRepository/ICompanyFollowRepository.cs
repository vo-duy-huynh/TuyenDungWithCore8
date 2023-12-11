using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface ICompanyFollowRepository : IRepository<CompanyFollow>
    {
        void Update(CompanyFollow obj);
    }
}
