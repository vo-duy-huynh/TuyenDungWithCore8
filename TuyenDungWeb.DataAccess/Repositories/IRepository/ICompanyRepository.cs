using TuyenDungWeb.Model;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        void Update(Company obj);
    }
}
