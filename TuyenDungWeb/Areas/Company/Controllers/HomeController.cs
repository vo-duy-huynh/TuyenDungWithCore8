using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;

namespace TuyenDungWeb.Areas.Company.Controllers
{
    [Area("Company")]
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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize]
        public IActionResult GetWishlistItems()
        {
            //get userId
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wishlistItems = _unitOfWork.WishList
                .GetWishListForUser(userId)
                .ToList();

            return Json(wishlistItems);
        }
        [Authorize]
        public IActionResult GetNotificationItems()
        {
            //get userId
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var jobPostTemps = _unitOfWork.JobPostTemp
                .GetAllNoApprove().ToList();

            return Json(jobPostTemps);
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddToWishlist(int jobPostId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if the item is already in the wishlist
            var WishList = _unitOfWork.WishList.GetFirstOrDefault(jobPostId, userId);
            List<WishList> wishlistItems;
            if (WishList != null)
            {
                // Item is already in the wishlist
                _unitOfWork.WishList.Remove(WishList);
                _unitOfWork.Save();
                //get userId
                wishlistItems = _unitOfWork.WishList
                   .GetWishListForUser(userId)
                   .ToList();
                return Json(new { success = true, message = "Bạn đã bỏ yêu thích thành công.", wishlistCount = wishlistItems.Count, wishlistItems = WishList });
            }

            // Item is not in the wishlist, so add it
            var newWishList = new WishList
            {
                JobPostId = jobPostId,
                Count = 1,
                ApplicationUserId = userId
            };

            _unitOfWork.WishList.Add(newWishList);
            _unitOfWork.Save();
            wishlistItems = _unitOfWork.WishList
                   .GetWishListForUser(userId)
                   .ToList();
            return Ok(new { message = "Bạn đã yêu thích thành công.", wishlistCount = wishlistItems.Count, wishlistItems = WishList });
        }
        [HttpPost]
        [Authorize]
        public IActionResult DeleteWishlistItem(int wishlistItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Check if the item is already in the wishlist
            var WishList = _unitOfWork.WishList.GetFirstOrDefault(wishlistItemId, userId);
            if (WishList != null)
            {
                // Item is already in the wishlist
                _unitOfWork.WishList.Remove(WishList);
                _unitOfWork.Save();
                var wishlistItems = _unitOfWork.WishList
                    .GetWishListForUser(userId)
                    .ToList();
                return Json(new { success = true, message = "Bạn đã bỏ yêu thích thành công.", wishlistCount = wishlistItems.Count, wishlistItems = WishList });
            }
            return Json(new { success = false, message = "Lỗi khi bỏ yêu thích." });
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}