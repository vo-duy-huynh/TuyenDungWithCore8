using System.ComponentModel.DataAnnotations;

namespace TuyenDungWeb.Models
{
    public class CompanyCareer
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int CareerId { get; set; }
        public Career Career { get; set; }
    }
}
