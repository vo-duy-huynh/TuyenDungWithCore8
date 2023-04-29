using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Model;
using TuyenDungWeb.Utility;

namespace TuyenDungWeb.MVC.Areas.Admin.Controllers
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
            List<Job> objJobList = _unitOfWork.Job.GetAll().ToList();
            return View(objJobList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Job obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Job.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Job created successfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        public IActionResult Edit(Guid? id)
        {
            if (id == null)
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
        public IActionResult Edit(Job obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Job.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Job updated successfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Job? JobFromDb = _unitOfWork.Job.Get(u => u.Id == id);

            if (JobFromDb == null)
            {
                return NotFound();
            }
            return View(JobFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Job? obj = _unitOfWork.Job.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Job.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Job deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
