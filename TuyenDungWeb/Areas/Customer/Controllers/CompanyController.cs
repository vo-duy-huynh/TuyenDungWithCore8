using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;
using TuyenDungWeb.Models.ViewModels;
using TuyenDungWeb.Utility;

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
            var objCompanyList = _unitOfWork.Company.GetAll().ToList();
            CompanyVM companyVM = new CompanyVM();
            var companyImages = _unitOfWork.CompanyImage.GetAll().ToList();
            companyVM.CompanyList = objCompanyList;
            companyVM.CompanyImageList = companyImages;
            companyVM.jobPostCount = new List<CompanyTemp>();
            foreach (var item in objCompanyList)
            {
                CompanyTemp companyTemp = new CompanyTemp();
                int count;
                try
                {
                    count = _unitOfWork.JobPost.GetAll(filter: u => u.CompanyId == item.Id).Count();
                }
                catch
                {

                    count = 0;
                }
                companyTemp.CompanyId = item.Id;
                companyTemp.jobPostCount = count;
                companyVM.jobPostCount.Add(companyTemp);
            }
            return View(companyVM);
        }
        public IActionResult TopCompany()
        {
            var objCompanyList = _unitOfWork.Company.GetAll(includeProperties: "CompanyImages").OrderBy(u => u.Id).Take(8).ToList();
            CompanyVM companyVM = new CompanyVM();
            var companyImages = _unitOfWork.CompanyImage.GetAll().ToList();
            companyVM.CompanyList = objCompanyList;
            companyVM.CompanyImageList = companyImages;
            companyVM.jobPostCount = new List<CompanyTemp>();
            foreach (var item in objCompanyList)
            {
                CompanyTemp companyTemp = new CompanyTemp();
                int count;
                try
                {
                    count = _unitOfWork.JobPost.GetAll(filter: u => u.CompanyId == item.Id).Count();
                }
                catch
                {

                    count = 0;
                }
                companyTemp.CompanyId = item.Id;
                companyTemp.jobPostCount = count;
                companyVM.jobPostCount.Add(companyTemp);
            }
            return View(companyVM);
        }
        [Route("company/{name}")]
        public IActionResult Detail(string name)
        {
            var company = _unitOfWork.Company.Get(filter: u => u.Name == name);
            var jobPosts = _unitOfWork.JobPost.GetAll(filter: u => u.CompanyId == company.Id).OrderByDescending(x => x.CreatedDate).ToList();

            var wishList = _unitOfWork.WishList.GetAll().ToList();
            var companyComments = _unitOfWork.CompanyComment.GetAll(filter: u => u.CompanyId == company.Id).ToList();
            var applicationUsers = _unitOfWork.ApplicationUser.GetAll().ToList();
            JobPostVM jobs = new()
            {
                JobPosts = jobPosts,
                Companies = _unitOfWork.Company.GetAll().ToList(),
                JobTypes = _unitOfWork.JobType.GetAll().ToList(),
                CompanyImages = _unitOfWork.CompanyImage.GetAll(filter: u => u.CompanyId == company.Id).ToList(),
                WistList = wishList,
                Company = company,
                CompanyComments = companyComments,
                ApplicationUsers = applicationUsers,
            };
            jobs.WishListVM = new List<WishListVM>();
            WishListVM wishListVM;
            //get userId is loging
            string userId;
            if (User.Identity.Name != null)
            {
                userId = _unitOfWork.ApplicationUser.GetAll().FirstOrDefault(u => u.Email == User.Identity.Name).Id;
            }
            else
            {
                userId = null;
            }
            foreach (var item in jobs.JobPosts)
            {
                WishList wishListTemp;
                wishListVM = new WishListVM();
                if (userId != null)
                {
                    wishListTemp = jobs.WistList.FirstOrDefault(u => u.JobPostId == item.Id && u.ApplicationUserId == userId);
                }
                else
                {
                    wishListTemp = null;
                }
                if (wishListTemp != null)
                {
                    wishListVM.JobPostId = item.Id;
                    wishListVM.Added = true;
                }
                else
                {
                    wishListVM.JobPostId = item.Id;
                    wishListVM.Added = false;
                }
                jobs.WishListVM.Add(wishListVM);
            }
            if (User.Identity.Name != null && _unitOfWork.ApplicationUser.GetAll().FirstOrDefault(u => u.Email == User.Identity.Name).FullName != null)
            {
                ViewBag.email = User.Identity.Name;
                ViewBag.fullname = _unitOfWork.ApplicationUser.GetAll().FirstOrDefault(u => u.Email == User.Identity.Name).FullName;
            }
            else
            {
                ViewBag.email = null;
                ViewBag.fullname = null;
            }
            return View(jobs);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.Role_Customer)]
        public IActionResult Comment(string name, string email, string rating, string comment, string companyIdInForm)
        {
            CompanyComment companyComment = new CompanyComment();
            companyComment.CompanyId = int.Parse(companyIdInForm);
            companyComment.DateAdded = DateTime.Now;
            companyComment.Description = comment;
            companyComment.Rate = double.Parse(rating);
            companyComment.UserId = _unitOfWork.ApplicationUser.GetAll().FirstOrDefault(u => u.Email == email).Id;
            _unitOfWork.CompanyComment.Add(companyComment);
            _unitOfWork.Save();
            TempData["success"] = "Thêm bình luận thành công!";
            return RedirectToAction("Index");
        }
        public IActionResult Follow(int id)
        {
            var company = _unitOfWork.Company.FirstOrDefault(id);
            var userId = _unitOfWork.ApplicationUser.GetAll().FirstOrDefault(u => u.Email == User.Identity.Name).Id;
            var follow = _unitOfWork.CompanyFollow.GetAll(filter: u => u.CompanyId == id && u.UserId == userId).FirstOrDefault();
            if (follow == null)
            {
                CompanyFollow follow1 = new CompanyFollow();
                follow1.CompanyId = id;
                follow1.UserId = userId;
                _unitOfWork.CompanyFollow.Add(follow1);
                _unitOfWork.Save();
                TempData["success"] = "Theo dõi thành công!";
            }
            else
            {
                _unitOfWork.CompanyFollow.Remove(follow);
                _unitOfWork.Save();
                TempData["success"] = "Bỏ theo dõi thành công!";
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = SD.Role_Admin)]
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