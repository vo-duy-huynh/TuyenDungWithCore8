namespace TuyenDungWeb.Models.ViewModels
{
    public class ApplyVM
    {
        public ProfileHeader ProfileHeader { get; set; }
        public IEnumerable<AdminNotification> Notifications { get; set; }
    }
}
