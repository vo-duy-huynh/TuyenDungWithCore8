using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface ICompanyImageRepository : IRepository<CompanyImage>
    {
        void Update(CompanyImage obj);
    }
}
