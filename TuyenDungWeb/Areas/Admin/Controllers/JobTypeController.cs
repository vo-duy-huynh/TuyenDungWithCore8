using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;
using TuyenDungWeb.Utility;

namespace TuyenDungWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class JobTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View(_unitOfWork.JobType.GetAll());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string Name)
        {
            JobType jobType = new JobType();
            jobType.Name = Name;
            if (Name == null)
            {
                TempData["error"] = "Lỗi";
                return RedirectToAction("Index");
            }
            else if (Name != null)
            {
                _unitOfWork.JobType.Add(jobType);
                _unitOfWork.Save();
                TempData["success"] = "Thêm thành công";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            JobType? JobFromDb = _unitOfWork.JobType.Get(u => u.Id == id);

            if (JobFromDb == null)
            {
                return NotFound();
            }
            return View(JobFromDb);
        }
        [HttpPost]
        public IActionResult Edit(string editId, string editName)
        {
            JobType obj = _unitOfWork.JobType.Get(u => u.Id == int.Parse(editId));
            if (obj != null)
            {
                obj.Name = editName;

            }

            _unitOfWork.JobType.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Cập nhật thành công";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            List<JobType> objJobList = _unitOfWork.JobType.GetAll().ToList();
            return Json(new { data = objJobList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var jobTypeToBeDeleted = _unitOfWork.JobType.Get(u => u.Id == id);
            if (jobTypeToBeDeleted == null)
            {
                return Json(new { success = false, message = "Lỗi trong khi xóa" });
            }

            _unitOfWork.JobType.Remove(jobTypeToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Xóa thành công" });
        }
    }
}
