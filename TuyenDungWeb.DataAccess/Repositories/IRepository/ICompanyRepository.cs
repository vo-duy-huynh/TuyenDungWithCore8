using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        void Update(Company obj);
        public Company FirstOrDefault(int? id);
        public Company GetById(int id);
    }
}
