using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;
using TuyenDungWeb.Models.ViewModels;

namespace TuyenDungWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class JobPostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public JobPostController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<JobPost> objJobPostList = _unitOfWork.JobPost.GetAll().ToList();
            // return Json(objJobPostList);
            return View(objJobPostList);
        }

        public IActionResult Upsert(int? id)
        {
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
        public IActionResult Upsert(JobPostVM JobPostVM, string CompanyId)
        {
            JobType existingJobType = new JobType();
            foreach (var selectedJobTypeItem in JobPostVM.SelectedJobTypes)
            {
                var selectedJobTypeId = int.Parse(selectedJobTypeItem);
                existingJobType = _unitOfWork.JobType.GetById(selectedJobTypeId);
            }
            JobPostVM.JobPost.JobTypeId = existingJobType.Id;

            foreach (var selectedCompanyItem in JobPostVM.SelectedCompanies)
            {
                JobPostVM.JobPost.CompanyId = int.Parse(selectedCompanyItem);
            }
            if (JobPostVM.JobPost.Id == 0)
            {
                _unitOfWork.JobPost.Add(JobPostVM.JobPost);
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
            List<JobPost> objJobPostList = _unitOfWork.JobPost.GetAll().ToList();
            return Json(new { data = objJobPostList });
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
