using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;
using TuyenDungWeb.Models.ViewModels;

namespace TuyenDungWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class JobPostTempController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public JobPostTempController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<JobPostTemp> objJobPostTempList = _unitOfWork.JobPostTemp.GetAllNoApprove().ToList();
            // return Json(objJobPostList);
            return View(objJobPostTempList);
        }

        public IActionResult Upsert(int? id)
        {
            JobPostVM JobPostVM = new()
            {
                JobList = _unitOfWork.Job.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                JobPostTemp = _unitOfWork.JobPostTemp.GetById(id),
                JobPost = new JobPost()
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
            JobPostVM.JobPostTemp.IsApprove = true;
            JobPostVM.JobPost.JobId = int.Parse(JobPostVM.SelectedJob);
            JobPostVM.JobPostTemp.JobId = int.Parse(JobPostVM.SelectedJob);
            if (JobPostVM.JobPost.Id == 0)
            {
                _unitOfWork.JobPost.Add(JobPostVM.JobPost);
                _unitOfWork.JobPostTemp.Update(JobPostVM.JobPostTemp);
            }
            else
            {
                _unitOfWork.JobPost.Update(JobPostVM.JobPost);
            }

            _unitOfWork.Save();

            TempData["success"] = "Thêm/ sửa thành công!";
            return RedirectToAction("Index");
        }


        //public IActionResult DeleteImage(int imageId)
        //{
        //var imageToBeDeleted = _unitOfWork.JobPostImage.Get(u => u.Id == imageId);
        //int JobPostId = imageToBeDeleted.JobPostId;
        //if (imageToBeDeleted != null)
        //{
        //    if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
        //    {
        //        var oldImagePath =
        //                       Path.Combine(_webHostEnvironment.WebRootPath,
        //                       imageToBeDeleted.ImageUrl.TrimStart('\\'));

        //        if (System.IO.File.Exists(oldImagePath))
        //        {
        //            System.IO.File.Delete(oldImagePath);
        //        }
        //    }

        //    _unitOfWork.JobPostImage.Remove(imageToBeDeleted);
        //    _unitOfWork.Save();

        //    TempData["success"] = "Xóa ảnh thành công";
        //}

        //return RedirectToAction(nameof(Upsert), new { id = JobPostId });
        //}
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
