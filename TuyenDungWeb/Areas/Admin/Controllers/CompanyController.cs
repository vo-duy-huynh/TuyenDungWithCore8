using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;
using TuyenDungWeb.Models.ViewModels;
using TuyenDungWeb.Utility;

namespace TuyenDungWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
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
            CompanyVM companyVM = new CompanyVM();
            List<int> tagIds = new List<int>();
            if (id == null || id == 0)
            {
                companyVM = new CompanyVM();
                companyVM.Company = new Models.Company();
                companyVM.Tags = _unitOfWork.Tag.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

            }
            else
            {
                var company = _unitOfWork.Company.FirstOrDefault(id);
                var companyImages = _unitOfWork.CompanyImage.GetCompanyImagesForCompany(id);
                company.Tags.ToList().ForEach(t => tagIds.Add(t.Id));
                companyVM.Tags = _unitOfWork.Tag.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                companyVM.Company = company;
                companyVM.Company.CompanyImages = companyImages;
                companyVM.tagIds = tagIds.ToArray();

            }
            return View(companyVM);

        }
        [HttpPost]
        public IActionResult Upsert(CompanyVM CompanyVM, List<IFormFile> files, string tagNames)
        {
            //split tagNames into array of tag names and add to database
            var tagNamesArray = tagNames.Split(", ");
            var selectedTags = new List<Tag>();
            foreach (var tagName in tagNamesArray)
            {
                var existingTag = _unitOfWork.Tag.GetFirstOrDefaultTagName(tagName);
                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }
            CompanyVM.Company.Tags = selectedTags;
            if (CompanyVM.Company.Id == 0)
            {
                _unitOfWork.Company.Add(CompanyVM.Company);
                TempData["success"] = "Thêm thành công!";
            }
            else
            {
                _unitOfWork.Company.Update(CompanyVM.Company);
                TempData["success"] = "Cập nhật thành công!";
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
                return Json(new { success = false, message = "Lỗi trong khi xóa" });
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