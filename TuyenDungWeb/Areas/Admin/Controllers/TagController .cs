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
        public IActionResult Create(Tag obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Tag.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Thêm thành công";
                return RedirectToAction("Index");
            }
            return View();

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
        public IActionResult Edit(Tag obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Tag.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Cập nhật thành công";
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
            Tag? TagFromDb = _unitOfWork.Tag.Get(u => u.Id == id);

            if (TagFromDb == null)
            {
                return NotFound();
            }
            return View(TagFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Tag? obj = _unitOfWork.Tag.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Tag.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Xóa thành công";
            return RedirectToAction("Index");
        }
    }
}
