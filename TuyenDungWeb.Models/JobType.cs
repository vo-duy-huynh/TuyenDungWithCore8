using System.ComponentModel.DataAnnotations;

namespace TuyenDungWeb.Models
{
    public class JobType
    {

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
