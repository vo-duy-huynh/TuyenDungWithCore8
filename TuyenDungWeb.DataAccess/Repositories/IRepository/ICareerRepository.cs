using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface ICareerRepository : IRepository<Career>
    {
        public Career GetFirstOrDefaultCareerName(string CareerName);
        public Career GetById(int id);
        public void Update(Career Career);
    }
}
