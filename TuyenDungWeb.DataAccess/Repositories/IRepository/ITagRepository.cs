using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface ITagRepository : IRepository<Tag>
    {
        public void Update(Tag tag);
    }
}
