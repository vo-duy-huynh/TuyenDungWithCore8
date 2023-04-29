using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace TuyenDungWeb.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Location { get; set; }
        public string? Content { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CompanyEmail { get; set; }
        [ValidateNever]
        public List<CompanyImage> CompanyImages { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
