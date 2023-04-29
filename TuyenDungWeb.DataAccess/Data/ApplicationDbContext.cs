using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TuyenDungWeb.Model;

namespace TuyenDungWeb.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<JobPost> JobPosts { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<JobPostComment> JobPostComments { get; set; }
        public DbSet<CompanyComment> CompanyComments { get; set; }
        public DbSet<JobPostLike> JobPostLikes { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobType> JobTypes { get; set; }

        public DbSet<WishList> WishLists { get; set; }
        public DbSet<CompanyImage> ProductImages { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ProfileHeader> ProfileHeaders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
