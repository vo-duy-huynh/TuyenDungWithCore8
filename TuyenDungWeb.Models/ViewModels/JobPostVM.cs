using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TuyenDungWeb.Models.ViewModels
{
    public class JobPostVM
    {
        public JobPost JobPost { get; set; }
        public JobPostTemp JobPostTemp { get; set; }
        public ProfileHeader ProfileHeader { get; set; }
        public Company Company { get; set; }
        public Career Career { get; set; }
        public List<Career> Careers { get; set; }
        public List<JobPost> JobPosts { get; set; }
        public List<JobPost> JobPostExpireds { get; set; }
        public List<JobPost> JobPostLikeJobTypes { get; set; }
        public List<ApplicationUser> ApplicationUsers { get; set; }
        public List<Company> Companies { get; set; }
        public List<ProfileHeader> ProfileHeaders { get; set; }
        public List<WishListVM> WishListVM { get; set; }
        public List<WishListVM> WishListVMExprireds { get; set; }

        public List<WishListVM> WishListVMLikeJobType { get; set; }
        public List<WishList> WistList { get; set; }
        public List<Tag> listTag { get; set; }
        public List<CompanyImage> CompanyImages { get; set; }
        public List<JobType> JobTypes { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> JobTypeList { get; set; }
        public IEnumerable<SelectListItem> JobList { get; set; }
        public IEnumerable<SelectListItem> CompanyList { get; set; }
        public IEnumerable<SelectListItem> CareerList { get; set; }
        public int[] tagIds { get; set; }
        public IEnumerable<SelectListItem> status { get; set; }
        public string SelectedJobType { get; set; }
        public string SelectedJob { get; set; }
        public string SelectedCompany { get; set; }
        public string SelectedCareer { get; set; }
        public IEnumerable<SelectListItem>? Tags { get; set; }
        [Required]
        public double Rate { get; set; }
        [Required]

        public string Description { get; set; }
        public int CompanyId { get; set; }

        public DateTime DateAdded { get; set; }

        public string Username { get; set; }

        [ValidateNever]
        public IEnumerable<CompanyComment>? CompanyComments { get; set; }
    }
}
