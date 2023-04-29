using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories
{
    public class JobPostRepository : Repository<JobPost>, IJobPostRepository
    {
        private ApplicationDbContext _db;
        public JobPostRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(JobPost obj)
        {
            var objFromDb = _db.JobPosts.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.PageTitle = obj.PageTitle;
                objFromDb.Content = obj.Content;
                objFromDb.Heading = obj.Heading;
                objFromDb.ShortDescription = obj.ShortDescription;
                objFromDb.UrlHandle = obj.UrlHandle;
                objFromDb.CreatedDate = obj.CreatedDate;
                objFromDb.EndDate = obj.EndDate;
                objFromDb.NumberOfRecruiting = obj.NumberOfRecruiting;
                objFromDb.Experience = obj.Experience;
                objFromDb.Gender = obj.Gender;
                objFromDb.Salary = obj.Salary;
                objFromDb.Location = obj.Location;
                objFromDb.Visible = obj.Visible;
                objFromDb.Status = obj.Status;
                //if (obj.ImageUrl != null)
                //{
                //    objFromDb.ImageUrl = obj.ImageUrl;
                //}
            }
        }
    }
}
