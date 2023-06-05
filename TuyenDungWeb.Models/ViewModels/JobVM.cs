using Microsoft.AspNetCore.Mvc.Rendering;

namespace TuyenDungWeb.Models.ViewModels
{
    public class JobVM
    {
        public List<Job> jobs { get; set; }
        public Job Job { get; set; }
        public IEnumerable<SelectListItem> CareeList { get; set; }
    }
}
