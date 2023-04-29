using TuyenDungWeb.Model;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface ICompanyImageRepository : IRepository<CompanyImage>
    {
        void Update(CompanyImage obj);
    }
}
