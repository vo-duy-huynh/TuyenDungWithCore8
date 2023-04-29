namespace TuyenDungWeb.Model
{
    public class JobPostComment
    {
        public System.Guid Id { get; set; }
        public string? Description { get; set; }
        public System.Guid JobPostId { get; set; }
        public System.Guid UserId { get; set; }
        public System.DateTime DateAdded { get; set; }

    }
}
