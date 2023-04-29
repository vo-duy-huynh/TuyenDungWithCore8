namespace TuyenDungWeb.Model
{
    public class ProfileHeader
    {


        public Guid Id { get; set; }
        public Nullable<System.Guid> JobPostId { get; set; }
        public string ApplicationUserId { get; set; }
        public System.DateTime ApplyDate { get; set; }
        public System.DateTime MatriculationDate { get; set; }
        public string Status { get; set; }
        public string SessionId { get; set; }
        public string PhoneNumber { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string CV { get; set; }
        public string Name { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public JobPost JobPost { get; set; }


    }
}
