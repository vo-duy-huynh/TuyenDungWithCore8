using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;
using TuyenDungWeb.Models.ViewModels;

namespace TuyenDungWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
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
                var jobPost = _unitOfWork.JobPost.FirstOrDefault(id);
                var jobType = _unitOfWork.JobType.GetById(jobPost.JobTypeId);
                jobPost.Tags.ToList().ForEach(t => tagIds.Add(t.Id));
                JobPostVM.JobPost = jobPost;
                JobPostVM.tagIds = tagIds.ToArray();
                JobPostVM.SelectedCompany = jobPost.CompanyId.ToString();
                JobPostVM.SelectedJob = jobPost.JobId.ToString();
                JobPostVM.SelectedJobType = jobPost.JobTypeId.ToString();
                return View(JobPostVM);
            }

        }
        [HttpPost]
        public IActionResult Upsert(JobPostVM JobPostVM)
        {
            JobType existingJobType = new JobType();
            JobPostVM.JobPost.JobTypeId = int.Parse(JobPostVM.SelectedJobType);
            JobPostVM.JobPost.CompanyId = int.Parse(JobPostVM.SelectedCompany);
            JobPostVM.JobPost.JobId = int.Parse(JobPostVM.SelectedJob);
            var tagNamesArray = JobPostVM.tagIds;
            var selectedTags = new List<Tag>();
            foreach (var tagName in tagNamesArray)
            {
                var existingTag = _unitOfWork.Tag.Get(filter: t => t.Id == tagName);
                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }
            JobPostVM.JobPost.Tags = selectedTags;
            if (JobPostVM.JobPost.Id == 0)
            {
                JobPostVM.JobPost.CreatedDate = DateTime.Now;
                _unitOfWork.JobPost.Add(JobPostVM.JobPost);
                TempData["success"] = "Thêm thành công!";
            }
            else
            {
                JobPostVM.JobPost.CreatedDate = _unitOfWork.JobPost.FirstOrDefault(JobPostVM.JobPost.Id).CreatedDate;
                _unitOfWork.JobPost.Update(JobPostVM.JobPost);
                TempData["success"] = "Cập nhật thành công!";
            }

            _unitOfWork.Save();
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
