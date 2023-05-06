using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<JobPost> JobPosts { get; set; }
        public DbSet<JobPostTemp> JobPostTemps { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<JobPostComment> JobPostComments { get; set; }
        public DbSet<CompanyComment> CompanyComments { get; set; }
        public DbSet<JobPostLike> JobPostLikes { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobType> JobTypes { get; set; }

        public DbSet<WishList> WishLists { get; set; }
        public DbSet<CompanyImage> CompanyImages { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ProfileHeader> ProfileHeaders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Job>().HasData(
               new Job { Id = 1, Name = "Tester", Note = "No" },
               new Job { Id = 2, Name = "Hacker", Note = "No" });
            modelBuilder.Entity<JobType>().HasData(
                new JobType { Id = 1, Name = "Fulltime" },
                new JobType { Id = 2, Name = "Parttime" });
            modelBuilder.Entity<Tag>().HasData(
                new Tag { Id = 1, Name = "C#" },
                new Tag { Id = 2, Name = "Java" },
                new Tag { Id = 3, Name = "Python" },
                new Tag { Id = 4, Name = "C++" },
                new Tag { Id = 5, Name = "PHP" },
                new Tag { Id = 6, Name = "Ruby" });
            modelBuilder.Entity<Company>().HasData(
                new Company { Id = 1, Name = "FPT", Location = "326 Hai Bà Trưng, Ba Đình, Hà Nội", Content = "No", PhoneNumber = "0987654321", CompanyEmail = "fpt@gmail.com" },
                new Company { Id = 2, Name = "Intel", Location = "327 Hiệp Phú, Thủ Đức, Hồ Chí Minh", Content = "No", PhoneNumber = "0987654321", CompanyEmail = "intel@gmail.com" },
                new Company { Id = 3, Name = "Vietcombank", Location = "123 Hải Thượng Lãn Ông, Đà Nẵng", Content = "No", PhoneNumber = "0987654321", CompanyEmail = "viecombank@gmail.com" }
                );



        }
    }
}
