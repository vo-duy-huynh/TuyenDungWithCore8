using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TuyenDungWeb.Models.ViewModels
{
    public class CompanyVM
    {
        public Company Company { get; set; }
        public List<Company> CompanyList { get; set; }
        public List<CompanyImage> CompanyImageList { get; set; }
        public List<CompanyTemp> jobPostCount { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? CompanyComments { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? CompanyImages { get; set; }
        public IEnumerable<SelectListItem> Companies { get; set; }
        [ValidateNever]
        // Collect Tag
        public int[] tagIds { get; set; }
        public IEnumerable<SelectListItem>? Tags { get; set; }

    }
}
