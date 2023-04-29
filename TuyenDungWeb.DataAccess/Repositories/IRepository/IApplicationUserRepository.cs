using TuyenDungWeb.Model;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        public void Update(ApplicationUser applicationUser);
    }
}
