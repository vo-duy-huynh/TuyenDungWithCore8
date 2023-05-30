using Microsoft.AspNetCore.Mvc;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;

namespace TuyenDungWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View(_unitOfWork.Tag.GetAll());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string Name, string DisplayName)
        {

            Tag tag = new Tag();
            tag.Name = Name;
            tag.DisplayName = DisplayName;
            if (Name == null)
            {
                TempData["error"] = "Lỗi";
                return RedirectToAction("Index");
            }
            else if (Name != null)
            {
                _unitOfWork.Tag.Add(tag);
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
            Tag? TagFromDb = _unitOfWork.Tag.Get(u => u.Id == id);
            //Tag? TagFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Tag? TagFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();

            if (TagFromDb == null)
            {
                return NotFound();
            }
            return View(TagFromDb);
        }
        [HttpPost]
        public IActionResult Edit(string editId, string editName, string editDisplayName)
        {

            Tag obj = _unitOfWork.Tag.Get(u => u.Id == int.Parse(editId));
            if (obj != null)
            {
                obj.Name = editName;
                obj.DisplayName = editDisplayName;

            }

            _unitOfWork.Tag.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Cập nhật thành công";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Tag.GetAll().ToList();
            return Json(new { data = allObj });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var tagToBeDeleted = _unitOfWork.Tag.Get(u => u.Id == id);
            if (tagToBeDeleted == null)
            {
                return Json(new { success = false, message = "Lỗi trong khi xóa" });
            }

            _unitOfWork.Tag.Remove(tagToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Xóa thành công" });
        }
    }
}
