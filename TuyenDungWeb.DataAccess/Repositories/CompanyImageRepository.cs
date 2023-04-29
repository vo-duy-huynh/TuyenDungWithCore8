using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Model;

namespace TuyenDungWeb.DataAccess.Repositories
{
    public class CompanyImageRepository : Repository<CompanyImage>, ICompanyImageRepository
    {
        private ApplicationDbContext _db;
        public CompanyImageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(CompanyImage obj)
        {
            _db.ProductImages.Update(obj);
        }
    }
}
