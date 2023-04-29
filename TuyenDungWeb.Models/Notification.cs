using System.ComponentModel.DataAnnotations;

namespace TuyenDungWeb.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public DateTime SendDate { get; set; }
        [Required]
        public string Content { get; set; }
        public bool? Status { get; set; }
        public string? ApplicationUser { get; set; }
    }
}
