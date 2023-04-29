using System.ComponentModel.DataAnnotations;

namespace TuyenDungWeb.Model
{
    public class WishList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int JobPostId { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        public JobPost JobPosts { get; set; }

    }
}
