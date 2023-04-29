namespace TuyenDungWeb.Model
{
    public class CompanyComment
    {
        public System.Guid Id { get; set; }
        public string Description { get; set; }
        public System.Guid CompanyId { get; set; }
        public System.Guid UserId { get; set; }
        public System.DateTime DateAdded { get; set; }
    }
}
