using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.DataAccess.Repositories;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repository
{
    public class CompanyFollowRepository : Repository<CompanyFollow>, ICompanyFollowRepository
    {
        private ApplicationDbContext _db;
        public CompanyFollowRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(CompanyFollow obj)
        {
            _db.CompanyFollows.Update(obj);
        }
    }
}