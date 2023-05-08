using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories
{
    public class JobPostTempRepository : Repository<JobPostTemp>, IJobPostTempRepository
    {
        private ApplicationDbContext _db;
        public JobPostTempRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public JobPostTemp GetById(int? id)
        {
            return _db.JobPostTemps.Find(id);
        }
        public int Count()
        {
            return _db.JobPostTemps.Count(n => !n.IsApprove);
        }
        public List<JobPostTemp> GetAllNoApprove()
        {
            return _db.JobPostTemps.Where(n => !n.IsApprove).ToList();
        }
        public void Update(JobPostTemp obj)
        {
            var objFromDb = _db.JobPostTemps.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Content = obj.Content;
                objFromDb.Heading = obj.Heading;
                objFromDb.ShortDescription = obj.ShortDescription;
                objFromDb.CreatedDate = obj.CreatedDate;
                objFromDb.EndDate = obj.EndDate;
                objFromDb.NumberOfRecruiting = obj.NumberOfRecruiting;
                objFromDb.Experience = obj.Experience;
                objFromDb.Gender = obj.Gender;
                objFromDb.Salary = obj.Salary;
                objFromDb.Location = obj.Location;
                objFromDb.JobId = obj.JobId;
                objFromDb.IsApprove = obj.IsApprove;
                objFromDb.Message = obj.Message;
                objFromDb.CompanyId = obj.CompanyId;
                objFromDb.JobTypeId = obj.JobTypeId;
            }
        }
    }
}
