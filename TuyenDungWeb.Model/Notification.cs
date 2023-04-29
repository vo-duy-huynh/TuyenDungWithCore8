namespace TuyenDungWeb.Model
{
    public class Notification
    {
        public System.Guid Id { get; set; }
        public Nullable<System.DateTime> SendDate { get; set; }
        public string Content { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.Guid> ApplicationUser { get; set; }
    }
}
