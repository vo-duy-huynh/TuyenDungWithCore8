using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using TuyenDungWeb.DataAccess.Repositories;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;
using TuyenDungWeb.Models.ViewModels;

namespace TuyenDungWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class JobPostTempController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly NotificationService _notificationService;

        public JobPostTempController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, NotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _notificationService = notificationService;
        }
        public IActionResult Index()
        {
            List<JobPostTemp> objJobPostTempList = _unitOfWork.JobPostTemp.GetAllNoApprove().ToList();
            // return Json(objJobPostList);
            return View(objJobPostTempList);
        }

        public IActionResult Upsert(int? id)
        {
            List<int> tagIds = new List<int>();
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
                JobList = _unitOfWork.Job.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Tags = _unitOfWork.Tag.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                JobPost = new JobPost(),
            };

            if (id == null || id == 0)
            {
                //create
                return View(JobPostVM);
            }
            else
            {
                var jobPost = _unitOfWork.JobPostTemp.Get(filter: j => j.Id == id);
                JobPostVM.JobPostTemp = jobPost;
                JobPostVM.SelectedCompany = jobPost.CompanyId.ToString();
                JobPostVM.SelectedJob = jobPost.JobId.ToString();
                JobPostVM.SelectedJobType = jobPost.JobTypeId.ToString();
                return View(JobPostVM);
            }

        }
        [HttpPost]
        public IActionResult Upsert(JobPostVM JobPostVM, string UrlHandle)
        {
            JobPostVM.JobPost = new JobPost();
            JobType existingJobType = new JobType();
            JobPostVM.JobPost.Heading = JobPostVM.JobPostTemp.Heading;
            JobPostVM.JobPost.ShortDescription = JobPostVM.JobPostTemp.ShortDescription;
            JobPostVM.JobPost.Content = JobPostVM.JobPostTemp.Content;
            JobPostVM.JobPost.Salary = JobPostVM.JobPostTemp.Salary;
            JobPostVM.JobPost.Experience = JobPostVM.JobPostTemp.Experience;
            JobPostVM.JobPost.NumberOfRecruiting = JobPostVM.JobPostTemp.NumberOfRecruiting;
            JobPostVM.JobPost.CreatedDate = JobPostVM.JobPostTemp.CreatedDate;
            JobPostVM.JobPost.EndDate = JobPostVM.JobPostTemp.EndDate;
            JobPostVM.JobPost.Gender = JobPostVM.JobPostTemp.Gender;
            JobPostVM.JobPost.JobTypeId = JobPostVM.JobPostTemp.JobTypeId;
            JobPostVM.JobPost.CompanyId = JobPostVM.JobPostTemp.CompanyId;
            JobPostVM.JobPost.Location = JobPostVM.JobPostTemp.Location;
            JobPostVM.JobPost.Status = true;
            JobPostVM.JobPost.Visible = true;
            JobPostVM.JobPost.UrlHandle = UrlHandle;
            JobPostVM.JobPost.UserPostId = JobPostVM.JobPostTemp.UserIdSend;
            JobPostVM.JobPostTemp.IsApprove = true;
            JobPostVM.JobPost.JobId = int.Parse(JobPostVM.SelectedJob);
            JobPostVM.JobPost.JobTypeId = int.Parse(JobPostVM.SelectedJobType);
            JobPostVM.JobPostTemp.JobId = int.Parse(JobPostVM.SelectedJob);
            JobPostVM.JobPostTemp.UserIdReceive = JobPostVM.JobPostTemp.UserIdSend;
            JobPostVM.JobPostTemp.JobPostId = _unitOfWork.JobPost.GetNextId();
            JobPostVM.JobPostTemp.UserIdSend = User.FindFirstValue(ClaimTypes.NameIdentifier);
            JobPostVM.JobPostTemp.Message = JobPostVM.JobPost.Heading.ToLower() + " đã được duyệt";
            if (JobPostVM.JobPost.Id == 0)
            {
                _unitOfWork.JobPost.Add(JobPostVM.JobPost);
                _unitOfWork.Save();
                _unitOfWork.JobPostTemp.Update(JobPostVM.JobPostTemp);
                _unitOfWork.Save();
                _notificationService.CreateNotificationForCompanyAfterAdmin(JobPostVM.JobPostTemp.UserIdReceive);
            }
            else
            {
                var tagIdsArray = JobPostVM.tagIds;
                foreach (var tagId in tagIdsArray)
                {
                    var tag = _unitOfWork.Tag.Get(filter: t => t.Id == tagId);
                    JobPostVM.JobPost.Tags.Add(tag);
                }
                _unitOfWork.JobPost.Update(JobPostVM.JobPost);
                _unitOfWork.Save();
            }



            TempData["success"] = "Cập nhật thành công!";
            return RedirectToAction("Index");
        }
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<JobPostTemp> objJobPostList = _unitOfWork.JobPostTemp.GetAllNoApprove().ToList();
            return Json(new { data = objJobPostList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var JobPostToBeDeleted = _unitOfWork.JobPostTemp.Get(u => u.Id == id);
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


            _unitOfWork.JobPostTemp.Remove(JobPostToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Xóa thành công" });
        }

        #endregion
    }
}
