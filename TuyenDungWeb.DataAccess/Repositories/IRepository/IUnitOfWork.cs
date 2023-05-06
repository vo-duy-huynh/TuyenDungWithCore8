namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface IUnitOfWork
    {
        IJobTypeRepository JobType { get; }
        IJobPostRepository JobPost { get; }
        IJobPostTempRepository JobPostTemp { get; }
        ICompanyRepository Company { get; }
        ICompanyCommentRepository CompanyComment { get; }
        ITagRepository Tag { get; }
        IJobRepository Job { get; }
        IWishListRepository WishList { get; }
        IApplicationUserRepository ApplicationUser { get; }
        INotificationRepository Notification { get; }
        IProfileHeaderRepository ProfileHeader { get; }
        ICompanyImageRepository CompanyImage { get; }
        void Save();
    }
}
