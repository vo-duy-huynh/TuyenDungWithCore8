using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface ITagRepository : IRepository<Tag>
    {

        public Tag GetById(int id);
        public void Update(Tag tag);
    }
}
