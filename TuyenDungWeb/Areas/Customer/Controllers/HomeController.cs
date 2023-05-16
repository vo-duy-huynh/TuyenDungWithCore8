using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;
using TuyenDungWeb.Models.ViewModels;

namespace TuyenDungWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var jobPosts = _unitOfWork.JobPost.GetAll().ToList();
            var wishList = _unitOfWork.WishList.GetAll().ToList();
            JobPostVM jobs = new()
            {
                JobPosts = jobPosts,
                Companies = _unitOfWork.Company.GetAll().ToList(),
                JobTypes = _unitOfWork.JobType.GetAll().ToList(),
                CompanyImages = _unitOfWork.CompanyImage.GetAll().ToList(),
                WistList = wishList,
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
            return View(jobs);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}