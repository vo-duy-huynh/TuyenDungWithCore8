using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuyenDungWeb.Models
{
    public class ProfileHeader
    {


        public int Id { get; set; }
        public int JobPostId { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        public DateTime ApplyDate { get; set; }
        public DateTime MatriculationDate { get; set; }
        public string? Status { get; set; }
        public string? SessionId { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string CV { get; set; }
        public string? Name { get; set; }
    }
}
