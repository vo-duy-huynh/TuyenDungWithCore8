using System.ComponentModel.DataAnnotations;

namespace TuyenDungWeb.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public virtual ICollection<JobPost> JobPosts { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
    }
}
