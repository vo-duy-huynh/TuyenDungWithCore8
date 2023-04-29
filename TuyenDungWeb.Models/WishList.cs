using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuyenDungWeb.Models
{
    public class WishList
    {
        public int Id { get; set; }
        public int JobPostId { get; set; }
        [ForeignKey("JobPostId")]
        [ValidateNever]
        public JobPost JobPost { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public string? ApplicationUserId { get; set; }

    }
}
