namespace TuyenDungWeb.Models.ViewModels
{
    public class CompanyCommentVM
    {
        public double Rate { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public DateTime DateAdded { get; set; }
        public string Username { get; set; }
    }
}
