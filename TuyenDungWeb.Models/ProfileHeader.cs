using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuyenDungWeb.Models
{
    public class ProfileHeader
    {


        public int Id { get; set; }
        public int JobPostId { get; set; }
        [ForeignKey("JobPostId")]
        [ValidateNever]
        public JobPost JobPost { get; set; }
        [Required]
        public string JobTitle { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public string? UserReceiveId { get; set; }
        public DateTime ApplyDate { get; set; }
        public DateTime? MatriculationDate { get; set; }
        public string? Status { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string CV { get; set; }
        public string CoverLetter { get; set; }
        public string? Message { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
