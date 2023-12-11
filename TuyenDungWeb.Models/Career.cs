using System.ComponentModel.DataAnnotations;

namespace TuyenDungWeb.Models
{
    public class Career
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public ICollection<CompanyCareer> CompanyCareers { get; set; }
    }
}
