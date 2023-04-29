using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface IProfileHeaderRepository : IRepository<ProfileHeader>
    {
        void Update(ProfileHeader obj);
        void UpdateStatus(int id, string Status);
        void UpdateToSession(int id, string session);
    }
}
