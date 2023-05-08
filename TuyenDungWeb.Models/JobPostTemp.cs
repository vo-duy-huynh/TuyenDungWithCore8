using System.ComponentModel.DataAnnotations;

namespace TuyenDungWeb.Models
{
    public class JobPostTemp
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Heading { get; set; }
        [Required]
        public string Content { get; set; }
        public string? ShortDescription { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        public int NumberOfRecruiting { get; set; }
        public double? Experience { get; set; }
        public string? Gender { get; set; }
        public string? Salary { get; set; }
        public string? Location { get; set; }
        public int JobTypeId { get; set; }
        public int CompanyId { get; set; }
        public int? JobId { get; set; }
        public string Message { get; set; }
        public bool IsApprove { get; set; }
    }
}
