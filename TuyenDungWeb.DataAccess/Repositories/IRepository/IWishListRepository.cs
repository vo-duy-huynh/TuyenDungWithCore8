using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface IWishListRepository : IRepository<WishList>
    {
        public WishList GetFirstOrDefault(int? jobPostId, string? ApplicationUserId);
        public List<WishList> GetWishListForUser(string? userId);
        void Update(WishList obj);
    }
}
