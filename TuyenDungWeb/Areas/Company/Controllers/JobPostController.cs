using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using TuyenDungWeb.DataAccess.Repositories;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;
using TuyenDungWeb.Models.ViewModels;
using TuyenDungWeb.Utility;

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
        public IActionResult Detail(int? id)
        {
            var jobPosts = _unitOfWork.JobPost.Get(u => u.Id == id);
            var company = _unitOfWork.Company.FirstOrDefault(id);
            var wishList = _unitOfWork.WishList.GetAll().ToList();
            var companyComments = _unitOfWork.CompanyComment.GetAll(filter: u => u.CompanyId == jobPosts.CompanyId).ToList();
            var applicationUsers = _unitOfWork.ApplicationUser.GetAll().ToList();
            JobPostVM jobs = new()
            {
                JobPost = jobPosts,
                Companies = _unitOfWork.Company.GetAll().ToList(),
                JobTypes = _unitOfWork.JobType.GetAll().ToList(),
                CompanyImages = _unitOfWork.CompanyImage.GetAll(filter: u => u.CompanyId == jobPosts.CompanyId).ToList(),
                WistList = wishList,
                Company = company,
                CompanyComments = companyComments,
                ApplicationUsers = applicationUsers,
            };
            jobs.WishListVM = new List<WishListVM>();
            WishListVM wishListVM;
            //get userId is loging
            string userId;
            if (User.Identity.Name != null)
            {
                userId = _unitOfWork.ApplicationUser.GetAll().FirstOrDefault(u => u.Email == User.Identity.Name).Id;
            }
            else
            {
                userId = null;
            }
            foreach (var item in jobs.JobPosts)
            {
                WishList wishListTemp;
                wishListVM = new WishListVM();
                if (userId != null)
                {
                    wishListTemp = jobs.WistList.FirstOrDefault(u => u.JobPostId == item.Id && u.ApplicationUserId == userId);
                }
                else
                {
                    wishListTemp = null;
                }
                if (wishListTemp != null)
                {
                    wishListVM.JobPostId = item.Id;
                    wishListVM.Added = true;
                }
                else
                {
                    wishListVM.JobPostId = item.Id;
                    wishListVM.Added = false;
                }
                jobs.WishListVM.Add(wishListVM);
            }
            if (User.Identity.Name != null && _unitOfWork.ApplicationUser.GetAll().FirstOrDefault(u => u.Email == User.Identity.Name).FullName != null)
            {
                ViewBag.email = User.Identity.Name;
                ViewBag.fullname = _unitOfWork.ApplicationUser.GetAll().FirstOrDefault(u => u.Email == User.Identity.Name).FullName;
            }
            else
            {
                ViewBag.email = null;
                ViewBag.fullname = null;
            }
            return View(jobs);

        }
        [Authorize(Roles = SD.Role_Company)]
        public IActionResult Upsert(int? id)
        {
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
        [Authorize(Roles = SD.Role_Company)]
        public IActionResult Upsert(JobPostVM JobPostVM, string CompanyId)
        {
            JobPostVM.JobPostTemp.JobTypeId = int.Parse(JobPostVM.SelectedJobType.ToString());
            JobPostVM.JobPostTemp.JobId = 1;
            JobPostVM.JobPostTemp.CreatedDate = DateTime.Now;
            JobPostVM.JobPostTemp.IsApprove = false;
            var companyName = _unitOfWork.Company.GetById(int.Parse(CompanyId)).Name;
            var message = $"Công ty {companyName} đã đăng tin và đang chờ chấp thuận.";
            var userIdSend = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userIdRecieve = _unitOfWork.ApplicationUser.GetAll().Where(u => u.Email == "admin@gmail.com").FirstOrDefault().Id;
            JobPostVM.JobPostTemp.Message = message;
            JobPostVM.JobPostTemp.UserIdSend = userIdSend;
            JobPostVM.JobPostTemp.UserIdReceive = userIdRecieve;

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
            _notificationService.CreateAdminNotification(message, notificationCount, userIdRecieve);
            TempData["success"] = "Chờ duyệt bạn nhé!";
            return RedirectToAction("Index", "Home", new { area = "Customer" });
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
