using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuyenDungWeb.Models
{
    public class JobPostLike
    {
        public int Id { get; set; }
        public int JobPostId { get; set; }
        [ForeignKey("JobPostId")]
        [ValidateNever]
        public JobPost JobPost { get; set; }
        public string? UserId { get; set; }

    }
}
