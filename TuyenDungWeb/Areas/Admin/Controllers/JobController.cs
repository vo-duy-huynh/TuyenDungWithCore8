using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;
using TuyenDungWeb.Utility;

namespace TuyenDungWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class JobController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View(_unitOfWork.Job.GetAll());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string Name, string Note)
        {
            if (Note == null)
            {
                Note = "";
            }
            Job job = new Job();
            job.Name = Name;
            job.Note = Note;
            if (Name == null)
            {
                TempData["error"] = "Lỗi";
                return RedirectToAction("Index");
            }
            else if (Name != null)
            {
                _unitOfWork.Job.Add(job);
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
            Job? JobFromDb = _unitOfWork.Job.Get(u => u.Id == id);
            //Job? JobFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Job? JobFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();

            if (JobFromDb == null)
            {
                return NotFound();
            }
            return View(JobFromDb);
        }
        [HttpPost]
        public IActionResult Edit(string editId, string editName, string editNote)
        {
            if (editNote == null)
            {
                editNote = "";
            }
            Job obj = _unitOfWork.Job.Get(u => u.Id == int.Parse(editId));
            if (obj != null)
            {
                obj.Name = editName;
                obj.Note = editNote;

            }

            _unitOfWork.Job.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Cập nhật thành công";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Job> objJobList = _unitOfWork.Job.GetAll().ToList();
            return Json(new { data = objJobList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var jobToBeDeleted = _unitOfWork.Job.Get(u => u.Id == id);
            if (jobToBeDeleted == null)
            {
                return Json(new { success = false, message = "Lỗi trong khi xóa" });
            }

            _unitOfWork.Job.Remove(jobToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Xóa thành công" });
        }
    }
}
