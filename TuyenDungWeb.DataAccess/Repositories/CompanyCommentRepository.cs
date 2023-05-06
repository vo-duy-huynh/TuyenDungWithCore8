using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.DataAccess.Repositories;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repository
{
    public class CompanyCommentRepository : Repository<CompanyComment>, ICompanyCommentRepository
    {
        private ApplicationDbContext _db;
        public CompanyCommentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(CompanyComment obj)
        {
            _db.CompanyComments.Update(obj);
        }
    }
}