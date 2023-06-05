﻿namespace TuyenDungWeb.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ApplicationUser Admin { get; set; }
        public ICollection<MessageChat> MessageChats { get; set; }
    }
}
