using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using TuyenDungWeb.DataAccess.Repositories;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;
using TuyenDungWeb.Models.ViewModels;

namespace TuyenDungWeb.Areas.Company.Controllers
{
    [Area("Company")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class JobPostController : Controller
    {
        private readonly NotificationService _notificationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public JobPostController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, NotificationService notificationService)
        {
            _notificationService = notificationService;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Upsert(int? id)
        {
            //get iduser
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userInCompany = _unitOfWork.ApplicationUser.GetCompanyId(userId);
            int? companyId = userInCompany.CompanyId;
            ViewBag.CompanyId = companyId;
            JobPostVM JobPostVM = new()
            {
                JobTypeList = _unitOfWork.JobType.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                CompanyList = _unitOfWork.Company.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                JobPostTemp = new JobPostTemp()
            };
            if (id == null || id == 0)
            {
                //create
                return View(JobPostVM);
            }
            else
            {
                return View(JobPostVM);
            }

        }
        [HttpPost]
        public IActionResult Upsert(JobPostVM JobPostVM, string CompanyId)
        {
            JobType existingJobType = new JobType();
            //int jobTypeId = int.Parse(JobPostVM.SelectedJobTypes[0].ToString());
            //existingJobType = _unitOfWork.JobType.GetById(jobTypeId);
            JobPostVM.JobPostTemp.JobTypeId = existingJobType.Id;
            JobPostVM.JobPostTemp.JobId = 1;

            JobPostVM.JobPostTemp.CreatedDate = DateTime.Now;
            JobPostVM.JobPostTemp.IsApprove = false;
            var companyName = _unitOfWork.Company.GetById(int.Parse(CompanyId)).Name;
            var message = $"Công ty {companyName} đã đăng tin và đang chờ chấp thuận.";
            JobPostVM.JobPostTemp.Message = message;
            if (CompanyId == null)
            {
                JobPostVM.JobPostTemp.CompanyId = int.Parse(JobPostVM.SelectedCompany);
            }
            if (CompanyId != null)
            {
                JobPostVM.JobPostTemp.CompanyId = int.Parse(CompanyId);
            }
            if (JobPostVM.JobPostTemp.Id == 0)
            {
                _unitOfWork.JobPostTemp.Add(JobPostVM.JobPostTemp);
            }
            else
            {
                _unitOfWork.JobPostTemp.Update(JobPostVM.JobPostTemp);
            }

            _unitOfWork.Save();
            // create new notification for admin
            var notificationCount = _unitOfWork.JobPostTemp.Count();
            _notificationService.CreateAdminNotification(message, notificationCount);
            return RedirectToAction(nameof(OrderConfirmation));
            //TempData["success"] = "Thêm/ sửa thành công!";
            //return RedirectToAction("Index", "Home");
        }
        public IActionResult OrderConfirmation()
        {
            return View();
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<JobPostTemp> objJobPostTempList = _unitOfWork.JobPostTemp.GetAll().ToList();
            return Json(new { data = objJobPostTempList.Count() });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var JobPostToBeDeleted = _unitOfWork.JobPost.Get(u => u.Id == id);
            if (JobPostToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            string JobPostPath = @"images\JobPosts\JobPost-" + id;
            string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, JobPostPath);

            if (Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }

                Directory.Delete(finalPath);
            }


            _unitOfWork.JobPost.Remove(JobPostToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Xóa thành công" });
        }

        #endregion
    }
}
