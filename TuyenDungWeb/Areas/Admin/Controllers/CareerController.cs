using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;
using TuyenDungWeb.Utility;

namespace TuyenDungWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CareerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CareerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View(_unitOfWork.Career.GetAll());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string Name)
        {

            Career Career = new Career();
            Career.Name = Name;
            if (Name == null)
            {
                TempData["error"] = "Lỗi";
                return RedirectToAction("Index");
            }
            else if (Name != null)
            {
                _unitOfWork.Career.Add(Career);
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
            Career? CareerFromDb = _unitOfWork.Career.Get(u => u.Id == id);
            //Career? CareerFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Career? CareerFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();

            if (CareerFromDb == null)
            {
                return NotFound();
            }
            return View(CareerFromDb);
        }
        [HttpPost]
        public IActionResult Edit(string editId, string editName)
        {

            Career obj = _unitOfWork.Career.Get(u => u.Id == int.Parse(editId));
            if (obj != null)
            {
                obj.Name = editName;

            }

            _unitOfWork.Career.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Cập nhật thành công";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Career.GetAll().ToList();
            return Json(new { data = allObj });
        }
        [HttpGet]
        public IActionResult GetOptions(string searchTerm)
        {
            // Retrieve search results based on the provided term
            // You can perform a database query or any other data retrieval logic here
            // Return the search results as a JSON array of objects with 'id' and 'text' properties

            var searchResults = _unitOfWork.Career.GetAll().Where(c => c.Name.Contains(searchTerm));

            var results = searchResults.Select(result => new
            {
                id = result.Id,
                text = result.Name
            });

            return Json(results);
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CareerToBeDeleted = _unitOfWork.Career.Get(u => u.Id == id);
            if (CareerToBeDeleted == null)
            {
                return Json(new { success = false, message = "Lỗi trong khi xóa" });
            }

            _unitOfWork.Career.Remove(CareerToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Xóa thành công" });
        }
    }
}
