using Microsoft.EntityFrameworkCore;
using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.DataAccess.Repositories;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public Company FirstOrDefault(int? id)
        {
            return _db.Companies.Include("Tags").FirstOrDefault(x => x.Id == id);
        }
        public Company GetById(int id)
        {
            var company = _db.Companies.Find(id);
            return company;
        }
        public void Update(Company obj)
        {
            var existingCompany = _db.Companies.Include(x => x.Tags).FirstOrDefault(u => u.Id == obj.Id);
            if (existingCompany != null)
            {
                existingCompany.Name = obj.Name;
                existingCompany.PhoneNumber = obj.PhoneNumber;
                existingCompany.CompanyEmail = obj.CompanyEmail;
                existingCompany.Content = obj.Content;
                existingCompany.Tags = obj.Tags;
                existingCompany.Location = obj.Location;
                existingCompany.IsApproved = obj.IsApproved;
                if (obj.CompanyImages != null)
                {
                    existingCompany.CompanyImages = obj.CompanyImages;
                }
            }
        }
    }
}