using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TuyenDungWeb.Models.ViewModels
{
    public class CompanyDetailVM
    {
        public Company Company { get; set; }
        public double Rate { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public DateTime DateAdded { get; set; }
        public string Username { get; set; }

        [ValidateNever]
        public IEnumerable<CompanyComment>? CompanyComments { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? CompanyImages { get; set; }
        public IEnumerable<SelectListItem> Companies { get; set; }
        [ValidateNever]
        // Collect Tag
        public string[] SelectedTags { get; set; } = Array.Empty<string>();
        public IEnumerable<SelectListItem>? Tags { get; set; }
    }
}
