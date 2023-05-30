using Microsoft.AspNetCore.Mvc;

namespace TuyenDungWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CVController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
