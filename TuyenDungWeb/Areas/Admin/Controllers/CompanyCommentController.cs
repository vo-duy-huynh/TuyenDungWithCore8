using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;
using TuyenDungWeb.Utility;

namespace TuyenDungWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyCommentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CompanyCommentController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<CompanyComment> companyComments = _unitOfWork.CompanyComment.GetAll(includeProperties: "Company").ToList();
            return View(companyComments);
        }
        //getall
        public IActionResult GetAll()
        {
            List<CompanyComment> companyComments = _unitOfWork.CompanyComment.GetAll(includeProperties: "Company").ToList();
            var applicationUsers = _unitOfWork.ApplicationUser.GetAll(includeProperties: "Company").ToList();
            List<object> modifiedCompanyComments = new List<object>();
            foreach (var comment in companyComments)
            {
                var matchingUser = applicationUsers.FirstOrDefault(user => user.Id == comment.UserId);
                var modifiedComment = new
                {
                    company = comment.Company,
                    //format datetime

                    dateAdded = comment.DateAdded.ToString("HH:mm:ss dd/MM/yyyy"),
                    rate = comment.Rate,
                    description = comment.Description,
                    id = comment.Id,
                    user = matchingUser != null ? new { id = matchingUser.Id, name = matchingUser.FullName } : null
                };

                modifiedCompanyComments.Add(modifiedComment);
            }

            return Json(new { data = modifiedCompanyComments });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToBeDeleted = _unitOfWork.CompanyComment.Get(u => u.Id == id);
            if (companyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Lỗi trong khi xóa" });
            }

            _unitOfWork.CompanyComment.Remove(companyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Xóa thành công" });
        }
    }
}
