﻿using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories.IRepository
{
    public interface IWishListRepository : IRepository<WishList>
    {
        void Update(WishList obj);
    }
}
