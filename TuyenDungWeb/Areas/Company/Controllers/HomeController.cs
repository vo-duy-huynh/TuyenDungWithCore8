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
            // get notification for user
            //var notificationItems = _unitOfWork.JobPostTemp
            //    .GetNotificationForUser(userId)
            //    .ToList();
            var notificationItems = _unitOfWork.JobPostTemp
                .GetAll().Where(u => u.UserIdReceive == userId).ToList();
            return Json(notificationItems);
        }
        [Authorize]
        public IActionResult GetNotificationItemsForCompanyAfterApply()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notificationItems = _unitOfWork.ProfileHeader
                .GetAll().OrderBy(n => n.ApplyDate).Where(u => u.UserReceiveId == userId && u.Status == "Pending").ToList();
            return Json(notificationItems);
        }
        [Authorize]
        public IActionResult GetNotificationItemsForCompanyAfterApprove()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notificationItems = _unitOfWork.JobPostTemp
                .GetAll().OrderBy(n => n.CreatedDate).Where(u => u.UserIdReceive == userId).ToList();
            return Json(notificationItems);
        }
        [Authorize]
        public IActionResult GetNotificationItemsForCompanyAfterApproveApply()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notificationItems = _unitOfWork.ProfileHeader
                .GetAll().OrderBy(n => n.ApplyDate).Where(u => u.UserReceiveId == userId && u.Status != "Pending").ToList();
            return Json(notificationItems);
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddToWishlist(int jobPostId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if the item is already in the wishlist
            var WishList = _unitOfWork.WishList.GetFirstOrDefault(jobPostId, userId);
            //get jobPost by id
            var jobPost = _unitOfWork.JobPost.Get(u => u.Id == jobPostId);
            if (WishList != null)
            {
                // Item is already in the wishlist
                _unitOfWork.WishList.Remove(WishList);
                _unitOfWork.Save();

                //get userId
                WishList wishlistItem = _unitOfWork.WishList
               .Get(u => u.ApplicationUserId == userId);
                return Json(new { success = true, message = "Bạn đã bỏ yêu thích thành công.", wishlistCount = wishlistItem.Count, wishlistItems = WishList, jobPost = jobPost });
            }

            // Item is not in the wishlist, so add it
            List<object> newWishLists = new List<object>();
            var newWishList = new WishList()
            {
                JobPostId = jobPostId,
                Count = 1,
                ApplicationUserId = userId
            };
            var itemInNewWishList = new
            {
                jobPostId = jobPostId,
                count = 1,
                applicationUserId = userId,
                urlHandle = jobPost.UrlHandle,
            };
            newWishLists.Add(itemInNewWishList);

            _unitOfWork.WishList.Add(newWishList);
            _unitOfWork.Save();
            List<object> wishlistItems = newWishLists;

            return Ok(new { message = "Bạn đã yêu thích thành công.", wishlistCount = wishlistItems.Count, wishlistItems = wishlistItems, jobPost = jobPost });
        }
        [HttpPost]
        [Authorize]
        public IActionResult DeleteWishlistItem(int wishlistItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Check if the item is already in the wishlist
            var WishList = _unitOfWork.WishList.GetFirstOrDefault(wishlistItemId, userId);
            //get jobPost by id
            var jobPost = _unitOfWork.JobPost.Get(u => u.Id == 1);
            if (WishList != null)
            {
                // Item is already in the wishlist
                _unitOfWork.WishList.Remove(WishList);
                _unitOfWork.Save();
                var wishlistItems = _unitOfWork.WishList
                    .GetWishListForUser(userId)
                    .ToList();
                return Json(new { success = true, message = "Bạn đã bỏ yêu thích thành công.", wishlistCount = wishlistItems.Count, wishlistItems = WishList, jobPost = jobPost });
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