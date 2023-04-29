using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace TuyenDungWeb.Model
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        [ValidateNever]
        public ICollection<JobPost> JobPosts { get; set; }

        public ICollection<Company> Companies { get; set; }


    }
}
