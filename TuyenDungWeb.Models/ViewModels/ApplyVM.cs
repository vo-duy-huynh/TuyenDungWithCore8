﻿namespace TuyenDungWeb.Models.ViewModels
{
    public class ApplyVM
    {
        public ProfileHeader ProfileHeader { get; set; }
        public IEnumerable<Notification> Notifications { get; set; }
    }
}
