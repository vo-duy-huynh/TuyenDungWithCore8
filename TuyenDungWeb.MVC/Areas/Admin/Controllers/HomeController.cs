using Microsoft.AspNetCore.Mvc;

namespace TuyenDungWeb.MVC.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
