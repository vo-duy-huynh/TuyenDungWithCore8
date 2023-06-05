using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TuyenDungWeb.DataAccess.Repositories;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models.ViewModels;

namespace TuyenDungWeb.Areas.Company.Controllers
{
    [Area("Company")]
    [Authorize]
    public class ProfileUserApplyController : Controller
    {
        private readonly NotificationService _notificationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProfileUserApplyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, NotificationService notificationService)
        {
            _notificationService = notificationService;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Update(int? id)
        {
            var profileHeader = _unitOfWork.ProfileHeader.Get(filter: u => u.Id == id);
            var jobPost = _unitOfWork.JobPost.Get(filter: u => u.Id == profileHeader.JobPostId);
            if (jobPost.NumberOfRecruiting <= 0)
            {
                TempData["error"] = "Số lượng tuyển dụng đã đủ";
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
            else if (jobPost.EndDate < DateTime.Now)
            {
                ViewBag.ShowAlert = true;
                ViewBag.SweetAlertIcon = "error";
                ViewBag.SweetAlertTitle = "Xin lỗi";
                ViewBag.SweetAlertText = "Thời gian nạp hồ sơ đã hết, có muốn tiếp nhận không?";
                ViewBag.SweetAlertFooter = "<a href=\"/Customer/Home/Index" + "\">Về trang chủ?</a>";
                var company = _unitOfWork.Company.Get(filter: u => u.Id == jobPost.CompanyId);
                //view
                JobPostVM jobPostVM = new JobPostVM()
                {
                    JobPost = jobPost,
                    ProfileHeader = profileHeader,
                    //select list status
                    status = new List<SelectListItem>()
                {
                new SelectListItem(){Text="Pending",Value="1"},
                new SelectListItem(){Text="Approved",Value="2"},
                new SelectListItem(){Text="Rejected",Value="3"},
                new SelectListItem(){Text="Interview",Value="4"},
                },
                    Company = company,
                };
                return View(jobPostVM);
            }
            else
            {
                ViewBag.ShowAlert = false;
                //get company
                var company = _unitOfWork.Company.Get(filter: u => u.Id == jobPost.CompanyId);
                //view
                JobPostVM jobPostVM = new JobPostVM()
                {
                    JobPost = jobPost,
                    ProfileHeader = profileHeader,
                    //select list status
                    status = new List<SelectListItem>()
                    {
                        new SelectListItem(){Text="Pending",Value="1"},
                        new SelectListItem(){Text="Approved",Value="2"},
                        new SelectListItem(){Text="Rejected",Value="3"},
                        new SelectListItem(){Text="Interview",Value="4"},
                    },
                    Company = company,
                };
                return View(jobPostVM);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(JobPostVM jobPostVM)
        {

            if (jobPostVM.ProfileHeader.Status == "2")
            {
                jobPostVM.ProfileHeader.MatriculationDate = DateTime.Now;
                jobPostVM.ProfileHeader.Status = "Approved";
                jobPostVM.ProfileHeader.Message = "Chúc mừng bạn đã trúng tuyển ";
                var jobPost = _unitOfWork.JobPost.Get(filter: u => u.Id == jobPostVM.ProfileHeader.JobPostId);
                jobPost.NumberOfRecruiting = jobPost.NumberOfRecruiting - 1;
                _unitOfWork.JobPost.Update(jobPost);
                _unitOfWork.ProfileHeader.Update(jobPostVM.ProfileHeader);
                _unitOfWork.Save();
            }
            else if (jobPostVM.ProfileHeader.Status == "3")
            {
                jobPostVM.ProfileHeader.Status = "Rejected";
                jobPostVM.ProfileHeader.Message = "Bạn có thể ứng tuyển vị trí khác ";
                _unitOfWork.ProfileHeader.Update(jobPostVM.ProfileHeader);
                _unitOfWork.Save();

            }
            else if (jobPostVM.ProfileHeader.Status == "4")
            {
                jobPostVM.ProfileHeader.Status = "Interview";
                jobPostVM.ProfileHeader.Message = "Hãy chuẩn bị để phỏng vấn vị trí ";
                _unitOfWork.ProfileHeader.Update(jobPostVM.ProfileHeader);
                _unitOfWork.Save();
            }
            TempData["success"] = "Đã thông báo tới ứng viên!";
            _notificationService.CreateNotificationForUser(jobPostVM.ProfileHeader.ApplicationUserId);
            return RedirectToAction("Index", "Home", new { area = "Customer" });

        }
    }
}
