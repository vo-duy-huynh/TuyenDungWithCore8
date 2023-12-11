using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.DataAccess.Repositories;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repository
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        private ApplicationDbContext _db;
        public TagRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public Tag GetById(int id)
        {
            var tag = _db.Tags.Find(id);
            return tag;
        }
        public Tag GetFirstOrDefaultTagName(string tagName)
        {
            var tag = _db.Tags.FirstOrDefault(x => x.Name == tagName);
            return tag;
        }
        public void Update(Tag obj)
        {
            _db.Tags.Update(obj);
        }
    }
}