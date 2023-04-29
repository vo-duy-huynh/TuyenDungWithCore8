using TuyenDungWeb.Model;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface IProfileHeaderRepository : IRepository<ProfileHeader>
    {
        void Update(ProfileHeader obj);
        void UpdateStatus(Guid id, string Status);
        void UpdateToSession(Guid id, string session);
    }
}
