using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface ICompanyCommentRepository : IRepository<CompanyComment>
    {
        void Update(CompanyComment obj);
    }
}
