using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;
using TuyenDungWeb.Models.ViewModels;

namespace TuyenDungWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
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

        public IActionResult Detail(int? id)
        {
            CompanyDetailVM CompanyDetailVM = new()
            {
                Tags = _unitOfWork.Tag.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Company = new TuyenDungWeb.Models.Company(),
                CompanyComments = _unitOfWork.CompanyComment.GetAll(filter: u => u.CompanyId == id, includeProperties: "Company"),
            };
            if (id == null || id == 0)
            {
                //create
                return View(CompanyDetailVM);
            }
            else
            {
                //update
                CompanyDetailVM.Company = _unitOfWork.Company.Get(u => u.Id == id, includeProperties: "CompanyImages");
                return View(CompanyDetailVM);
            }

        }
        [HttpPost]
        public IActionResult Insert(CompanyDetailVM CompanyDetailVM)
        {
            CompanyComment companyComment = new CompanyComment();
            companyComment.CompanyId = CompanyDetailVM.Company.Id;
            companyComment.DateAdded = DateTime.Now;
            companyComment.Description = CompanyDetailVM.Description;
            companyComment.Rate = 5;
            companyComment.UserId = "huynhpro";
            _unitOfWork.CompanyComment.Add(companyComment);
            _unitOfWork.Save();
            TempData["success"] = "Thêm bình luận thành công!";
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

            return RedirectToAction(nameof(Detail), new { id = CompanyId });
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