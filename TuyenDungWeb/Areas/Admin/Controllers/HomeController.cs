using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models.ViewModels;
using TuyenDungWeb.Utility;

namespace TuyenDungWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            JobPostVM jobPostVM = new JobPostVM()
            {
                JobPosts = (List<Models.JobPost>)_unitOfWork.JobPost.GetAll(includeProperties: "Company"),
                Companies = (List<Models.Company>)_unitOfWork.Company.GetAll(),
                JobTypes = (List<Models.JobType>)_unitOfWork.JobType.GetAll(),
                ApplicationUsers = (List<Models.ApplicationUser>)_unitOfWork.ApplicationUser.GetAll(includeProperties: "Company"),
                CompanyComments = (List<Models.CompanyComment>)_unitOfWork.CompanyComment.GetAll(includeProperties: "Company"),
                ProfileHeaders = (List<Models.ProfileHeader>)_unitOfWork.ProfileHeader.GetAll(includeProperties: "JobPost"),
            };
            return View(jobPostVM);
        }
        //get all user api return json
        public IActionResult GetAllUser()
        {
            var allObj = _unitOfWork.ApplicationUser.GetAll(includeProperties: "Company");
            return Json(new { data = allObj });
        }
        //get all company api return json
        public IActionResult GetAllCompany()
        {
            var allObj = _unitOfWork.Company.GetAll();
            return Json(new { data = allObj });
        }
        //get all profile status = approve api return json
        public IActionResult GetAllProfile()
        {
            var allObj = _unitOfWork.ProfileHeader.GetAll(includeProperties: "JobPost");
            return Json(new { data = allObj });
        }
        //get all companycomment api return json
        public IActionResult GetAllCompanyComment()
        {
            var allObj = _unitOfWork.CompanyComment.GetAll(includeProperties: "Company");
            return Json(new { data = allObj });
        }
    }
}
