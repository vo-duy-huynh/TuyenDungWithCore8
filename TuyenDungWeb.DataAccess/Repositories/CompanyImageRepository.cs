using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories
{
    public class CompanyImageRepository : Repository<CompanyImage>, ICompanyImageRepository
    {
        private ApplicationDbContext _db;
        public CompanyImageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public List<CompanyImage> GetCompanyImagesForCompany(int? companyId)
        {
            return _db.CompanyImages.Where(x => x.CompanyId == companyId).ToList();
        }

        public void Update(CompanyImage obj)
        {
            _db.CompanyImages.Update(obj);
        }
    }
}
