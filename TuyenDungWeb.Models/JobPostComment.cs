using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuyenDungWeb.Models
{
    public class JobPostComment
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public int JobPostId { get; set; }
        [ForeignKey("JobPostId")]
        [ValidateNever]
        public JobPost JobPost { get; set; }
        public string UserId { get; set; }
        public DateTime DateAdded { get; set; }

    }
}
