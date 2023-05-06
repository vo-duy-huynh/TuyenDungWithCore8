using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TuyenDungWeb.Models.ViewModels
{
    public class JobPostVM
    {
        public JobPost JobPost { get; set; }
        public JobPostTemp JobPostTemp { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> JobTypeList { get; set; }
        public IEnumerable<SelectListItem> CompanyList { get; set; }
        public string[] SelectedJobTypes { get; set; } = Array.Empty<string>();
        public string[] SelectedCompanies { get; set; } = Array.Empty<string>();
        public IEnumerable<SelectListItem>? Tags { get; set; }
    }
}
