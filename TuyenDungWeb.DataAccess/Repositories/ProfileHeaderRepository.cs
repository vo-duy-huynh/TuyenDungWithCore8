﻿using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repositories
{
    public class ProfileHeaderRepository : Repository<ProfileHeader>, IProfileHeaderRepository
    {
        private ApplicationDbContext _db;
        public ProfileHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(ProfileHeader obj)
        {
            _db.ProfileHeaders.Update(obj);
        }

        public void UpdateStatus(int id, string ProfileStatus)
        {
            var ProfileFromDb = _db.ProfileHeaders.FirstOrDefault(u => u.Id == id);
            if (ProfileFromDb != null)
            {
                ProfileFromDb.Status = ProfileStatus;
            }
        }

        public void UpdateToSession(int id, string session)
        {
            throw new NotImplementedException();
        }
    }
}
