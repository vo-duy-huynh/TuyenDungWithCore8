using Microsoft.AspNetCore.Mvc;
using TuyenDungWeb.DataAccess.Repositories;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;
using TuyenDungWeb.Models.ViewModels;

namespace TuyenDungWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class JobPostController : Controller
    {
        private readonly NotificationService _notificationService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;

        public JobPostController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, NotificationService notificationService)
        {
            _notificationService = notificationService;
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Apply(int id)
        {
            var objFromDb = _unitOfWork.JobPost.Get(filter: u => u.Id == id);
            var company = _unitOfWork.Company.Get(filter: u => u.Id == objFromDb.CompanyId);
            var user = _unitOfWork.ApplicationUser.Get(filter: u => u.UserName == User.Identity.Name);
            ProfileHeader profileHeader = new ProfileHeader();
            profileHeader.ApplicationUser = user;
            profileHeader.JobPost = objFromDb;
            JobPostVM jobPostVM = new JobPostVM()
            {
                JobPost = objFromDb,
                Company = company,
                ProfileHeader = profileHeader,
            };
            return View(jobPostVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //add mutipart file cv and cover letter for apply http post
        public IActionResult Apply(JobPostVM jobPostVM, IFormFile cv, IFormFile coverLetter)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (cv != null && cv.Length > 0)
            {
                var jobPost = _unitOfWork.JobPost.Get(filter: u => u.Id == jobPostVM.ProfileHeader.JobPostId);
                //check enddate and number of recruiting
                if (jobPost.EndDate < DateTime.Now || jobPost.NumberOfRecruiting <= 0)
                {
                    TempData["error"] = "Hết hạn tuyển dụng hoặc số lượng tuyển dụng đã đủ";
                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }
                else
                {
                    string cvFileName = Guid.NewGuid().ToString() + Path.GetExtension(cv.FileName);
                    string CVPath = @"cvs\";
                    string finalPath = Path.Combine(wwwRootPath, CVPath);

                    if (!Directory.Exists(finalPath))
                        Directory.CreateDirectory(finalPath);

                    using (var fileStream = new FileStream(Path.Combine(finalPath, cvFileName), FileMode.Create))
                    {
                        cv.CopyTo(fileStream);
                    }
                    var profileHeader = new ProfileHeader
                    {
                        JobPostId = jobPostVM.ProfileHeader.JobPostId,
                        Name = jobPostVM.ProfileHeader.Name,
                        Email = jobPostVM.ProfileHeader.Email,
                        PhoneNumber = jobPostVM.ProfileHeader.PhoneNumber,
                        CV = @"\" + CVPath + @"\" + cvFileName,
                        ApplyDate = DateTime.Now,
                        Status = "Pending",
                        ApplicationUserId = jobPostVM.ProfileHeader.ApplicationUserId,
                        City = jobPostVM.ProfileHeader.City,
                        UserReceiveId = jobPostVM.ProfileHeader.UserReceiveId,
                        JobTitle = _unitOfWork.JobPost.Get(filter: u => u.Id == jobPostVM.ProfileHeader.JobPostId).Heading,
                    };

                    if (coverLetter != null && coverLetter.Length > 0)
                    {
                        var coverLetterFileName = Guid.NewGuid().ToString() + Path.GetExtension(coverLetter.FileName);
                        string CoverLetterPath = @"letters\";
                        var coverLetterFilePath = Path.Combine(wwwRootPath, CoverLetterPath);
                        if (!Directory.Exists(coverLetterFilePath))
                            Directory.CreateDirectory(coverLetterFilePath);
                        using (var stream = new FileStream(Path.Combine(coverLetterFilePath, coverLetterFileName), FileMode.Create))
                        {
                            coverLetter.CopyTo(stream);
                        }
                        profileHeader.CoverLetter = @"\" + CoverLetterPath + @"\" + coverLetterFileName;
                    }

                    _unitOfWork.ProfileHeader.Add(profileHeader);
                    _unitOfWork.Save();
                    _notificationService.CreateNotificationForCompany(jobPostVM.ProfileHeader.UserReceiveId);
                    TempData["Success"] = "Ứng tuyển thành công, chờ công ty xác nhận nhé!!";
                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }
            }

            else
            {
                var objFromDb = _unitOfWork.JobPost.Get(filter: u => u.Id == jobPostVM.ProfileHeader.JobPostId);
                var company = _unitOfWork.Company.Get(filter: u => u.Id == objFromDb.CompanyId);
                var user = _unitOfWork.ApplicationUser.Get(filter: u => u.UserName == User.Identity.Name);
                ProfileHeader profileHeader = new ProfileHeader();
                profileHeader.ApplicationUser = user;
                profileHeader.JobPost = objFromDb;
                JobPostVM jobPostVM2 = new JobPostVM()
                {
                    JobPost = objFromDb,
                    Company = company,
                    ProfileHeader = profileHeader,
                };
                return View(jobPostVM2);
            }
        }


    }
}
