namespace TuyenDungWeb.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; }
    }
}
