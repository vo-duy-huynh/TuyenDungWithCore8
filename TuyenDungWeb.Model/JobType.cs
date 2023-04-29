namespace TuyenDungWeb.Model
{
    public class JobType
    {

        public System.Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<JobPost> JobPosts { get; set; }
    }
}
