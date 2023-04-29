using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TuyenDungWeb.Models.ViewModels
{
    public class CompanyVM
    {
        public Company Company { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CompanyComments { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CompanyImages { get; set; }
        public IEnumerable<SelectListItem> Companies { get; set; }
        [ValidateNever]
        // Collect Tag
        public string[] SelectedTags { get; set; } = Array.Empty<string>();
        public IEnumerable<SelectListItem> Tags { get; set; }

    }
}
