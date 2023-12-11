using Microsoft.EntityFrameworkCore;
using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories
{
    public class WishListRepository : Repository<WishList>, IWishListRepository
    {
        private ApplicationDbContext _db;
        public WishListRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public WishList GetFirstOrDefault(int? jobPostId, string? ApplicationUserId)
        {
            return _db.WishLists.FirstOrDefault(x => x.JobPostId == jobPostId && x.ApplicationUserId == ApplicationUserId);
        }
        public List<WishList> GetWishListForUser(string? userId)
        {
            return _db.WishLists.Include("JobPost").Where(x => x.ApplicationUserId == userId).ToList();
        }
        public void Update(WishList obj)
        {
            _db.WishLists.Update(obj);
        }
    }
}
