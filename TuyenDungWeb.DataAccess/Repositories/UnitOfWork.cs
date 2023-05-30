using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.DataAccess.Repository;

namespace TuyenDungWeb.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IJobTypeRepository JobType { get; private set; }
        public IJobPostRepository JobPost { get; private set; }
        public IJobPostTempRepository JobPostTemp { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public ICompanyCommentRepository CompanyComment { get; private set; }
        public ICompanyFollowRepository CompanyFollow { get; private set; }
        public ITagRepository Tag { get; private set; }
        public ICareerRepository Career { get; private set; }
        public IJobRepository Job { get; private set; }


        public IWishListRepository WishList { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IProfileHeaderRepository ProfileHeader { get; private set; }
        public INotificationRepository Notification { get; private set; }
        public ICompanyImageRepository CompanyImage { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            CompanyImage = new CompanyImageRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            WishList = new WishListRepository(_db);
            JobType = new JobTypeRepository(_db);
            JobPost = new JobPostRepository(_db);
            JobPostTemp = new JobPostTempRepository(_db);
            Company = new CompanyRepository(_db);
            CompanyComment = new CompanyCommentRepository(_db);
            CompanyFollow = new CompanyFollowRepository(_db);
            Tag = new TagRepository(_db);
            Career = new CareerRepository(_db);
            Job = new JobRepository(_db);
            ProfileHeader = new ProfileHeaderRepository(_db);
            Notification = new NotificationRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
