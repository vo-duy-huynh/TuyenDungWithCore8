namespace TuyenDungWeb.Model
{
    public class JobPost
    {
        public System.Guid Id { get; set; }
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string UrlHandle { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> NumberOfRecruiting { get; set; }
        public Nullable<double> Experience { get; set; }
        public string Gender { get; set; }
        public string Salary { get; set; }
        public string Location { get; set; }
        public Nullable<System.Guid> JobTypeId { get; set; }
        public Nullable<System.Guid> CompanyId { get; set; }
        public bool Visible { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.Guid> JobId { get; set; }

        public Company Company { get; set; }
        public JobType JobType { get; set; }
        public Job Job { get; set; }
        public ICollection<JobPostComment> JobPostComments { get; set; }
        public ICollection<JobPostLike> JobPostLikes { get; set; }
        public ICollection<ProfileHeader> ProfileHeaders { get; set; }
        public ICollection<WishList> WishLists { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
