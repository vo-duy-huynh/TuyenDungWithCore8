namespace TuyenDungWeb.Model
{
    public class CompanyImage
    {

        public System.Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public Nullable<System.Guid> CompanyId { get; set; }

        public Company Company { get; set; }
    }
}
