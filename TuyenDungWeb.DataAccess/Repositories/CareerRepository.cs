using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.DataAccess.Repositories;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repository
{
    public class CareerRepository : Repository<Career>, ICareerRepository
    {
        private ApplicationDbContext _db;
        public CareerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public Career GetById(int id)
        {
            var Career = _db.Careers.Find(id);
            return Career;
        }
        public Career GetFirstOrDefaultCareerName(string CareerName)
        {
            var Career = _db.Careers.FirstOrDefault(x => x.Name == CareerName);
            return Career;
        }
        public void Update(Career obj)
        {
            _db.Careers.Update(obj);
        }
    }
}