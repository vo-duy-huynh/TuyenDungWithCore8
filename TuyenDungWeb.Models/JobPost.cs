using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuyenDungWeb.Models
{
    public class JobPost
    {
        public int Id { get; set; }
        [Required]
        public string Heading { get; set; }
        [Required]
        public string Content { get; set; }
        public string? ShortDescription { get; set; }
        public string? UrlHandle { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        public int NumberOfRecruiting { get; set; }
        public double? Experience { get; set; }
        public string? Gender { get; set; }
        public string? Salary { get; set; }
        public string? Location { get; set; }
        public int? JobTypeId { get; set; }
        [ForeignKey("JobTypeId")]
        [ValidateNever]
        public JobType JobType { get; set; }
        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [ValidateNever]
        public Company Company { get; set; }
        public bool? Visible { get; set; }
        public bool? Status { get; set; }
        public string? UserPostId { get; set; }
        public int? JobId { get; set; }
        [ForeignKey("JobId")]
        [ValidateNever]
        public Job Job { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }

    }
}
