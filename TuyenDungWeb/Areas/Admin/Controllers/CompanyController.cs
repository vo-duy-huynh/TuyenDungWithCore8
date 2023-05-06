using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;
using TuyenDungWeb.Models.ViewModels;

namespace TuyenDungWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<TuyenDungWeb.Models.Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return View(objCompanyList);
        }

        public IActionResult Upsert(int? id)
        {
            CompanyVM CompanyVM = new()
            {
                Tags = _unitOfWork.Tag.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Company = new TuyenDungWeb.Models.Company()
            };
            if (id == null || id == 0)
            {
                //create
                return View(CompanyVM);
            }
            else
            {
                //update
                CompanyVM.Company = _unitOfWork.Company.Get(u => u.Id == id, includeProperties: "CompanyImages");
                return View(CompanyVM);
            }

        }
        [HttpPost]
        public IActionResult Upsert(CompanyVM CompanyVM, List<IFormFile> files)
        {
            // Map Tags from selected tags
            var selectedTags = new List<Tag>();
            foreach (var selectedTagItem in CompanyVM.SelectedTags)
            {
                var selectedTagId = int.Parse(selectedTagItem);
                var existingTag = _unitOfWork.Tag.GetById(selectedTagId);

                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }
            CompanyVM.Company.Tags = selectedTags;
            if (CompanyVM.Company.Id == 0)
            {
                _unitOfWork.Company.Add(CompanyVM.Company);
            }
            else
            {
                _unitOfWork.Company.Update(CompanyVM.Company);
            }

            _unitOfWork.Save();


            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (files != null)
            {

                foreach (IFormFile file in files)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string CompanyPath = @"images\Companys\Company-" + CompanyVM.Company.Id;
                    string finalPath = Path.Combine(wwwRootPath, CompanyPath);

                    if (!Directory.Exists(finalPath))
                        Directory.CreateDirectory(finalPath);

                    using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    CompanyImage CompanyImage = new()
                    {
                        ImageUrl = @"\" + CompanyPath + @"\" + fileName,
                        CompanyId = CompanyVM.Company.Id,
                    };

                    if (CompanyVM.Company.CompanyImages == null)
                        CompanyVM.Company.CompanyImages = new List<CompanyImage>();

                    CompanyVM.Company.CompanyImages.Add(CompanyImage);

                }

                _unitOfWork.Company.Update(CompanyVM.Company);
                _unitOfWork.Save();
            }


            TempData["success"] = "Thêm/ sửa thành công!";
            return RedirectToAction("Index");
        }


        public IActionResult DeleteImage(int imageId)
        {
            var imageToBeDeleted = _unitOfWork.CompanyImage.Get(u => u.Id == imageId);
            int CompanyId = imageToBeDeleted.CompanyId;
            if (imageToBeDeleted != null)
            {
                if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
                {
                    var oldImagePath =
                                   Path.Combine(_webHostEnvironment.WebRootPath,
                                   imageToBeDeleted.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitOfWork.CompanyImage.Remove(imageToBeDeleted);
                _unitOfWork.Save();

                TempData["success"] = "Xóa ảnh thành công";
            }

            return RedirectToAction(nameof(Upsert), new { id = CompanyId });
        }
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<TuyenDungWeb.Models.Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if (companyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            string CompanyPath = @"images\Companys\Company-" + id;
            string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, CompanyPath);

            if (Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }

                Directory.Delete(finalPath);
            }


            _unitOfWork.Company.Remove(companyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Xóa thành công" });
        }

        #endregion
    }
}