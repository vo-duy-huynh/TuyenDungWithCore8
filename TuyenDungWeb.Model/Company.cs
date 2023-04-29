namespace TuyenDungWeb.Model
{
    public class Company
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Content { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyEmail { get; set; }
        public ICollection<CompanyComment> CompanyComments { get; set; }
        public ICollection<JobPost> JobPosts { get; set; }
        public ICollection<CompanyImage> CompanyImages { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
