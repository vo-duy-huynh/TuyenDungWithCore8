﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using TuyenDungWeb.DataAccess.Repositories;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Models;
using TuyenDungWeb.Models.ViewModels;

namespace TuyenDungWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly NotificationService _notificationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, NotificationService notificationService)
        {
            _notificationService = notificationService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var jobPosts = _unitOfWork.JobPost.GetAll().OrderByDescending(u => u.CreatedDate).Where(u => u.EndDate >= DateTime.Now).ToList();
            var jobPostExpireds = _unitOfWork.JobPost.GetAll().OrderByDescending(u => u.CreatedDate).Where(u => u.EndDate < DateTime.Now).ToList();
            var wishList = _unitOfWork.WishList.GetAll().ToList();
            JobPostVM jobs = new()
            {
                JobPosts = jobPosts,
                JobPostExpireds = jobPostExpireds,
                Companies = _unitOfWork.Company.GetAll().ToList(),
                listTag = _unitOfWork.Tag.GetAll(includeProperties: "JobPosts,Companies").ToList(),
                JobTypeList = _unitOfWork.JobType.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                CompanyList = _unitOfWork.Company.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                CareerList = _unitOfWork.Career.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                JobTypes = _unitOfWork.JobType.GetAll().ToList(),
                CompanyImages = _unitOfWork.CompanyImage.GetAll().ToList(),
                WistList = wishList,
            };
            jobs.WishListVM = new List<WishListVM>();
            jobs.WishListVMExprireds = new List<WishListVM>();
            WishListVM wishListVM;

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
            foreach (var item in jobs.JobPostExpireds)
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
                jobs.WishListVMExprireds.Add(wishListVM);
            }
            return View(jobs);
        }
        [Route("ViecLam/{url?}")]
        public IActionResult Detail(string url)
        {
            if (url == null)
            {
                return NotFound();
            }
            var jobPost = _unitOfWork.JobPost.Get(filter: u => u.UrlHandle == url, includeProperties: "Tags");
            var company = _unitOfWork.Company.FirstOrDefault(jobPost.CompanyId);
            var wishList = _unitOfWork.WishList.GetAll().ToList();
            //get all post jobs like job
            var jobPostsLikeTagsTemp = _unitOfWork.JobPost.GetAll(filter: u => u.Id != jobPost.Id && u.JobId == jobPost.JobId, includeProperties: "Tags").ToList();
            //get all jobPostLikeJobTypes
            var jobPosJobPostLikeJobTypes = _unitOfWork.JobPost.GetAll(filter: u => u.Id != jobPost.Id && u.JobTypeId == jobPost.JobTypeId).ToList();

            JobPostVM jobs = new()
            {
                JobPost = jobPost,
                JobPosts = jobPostsLikeTagsTemp,
                JobPostLikeJobTypes = jobPosJobPostLikeJobTypes,
                Companies = _unitOfWork.Company.GetAll().ToList(),
                JobTypes = _unitOfWork.JobType.GetAll().ToList(),
                CompanyImages = _unitOfWork.CompanyImage.GetAll().ToList(),
                WistList = wishList,
                Company = company,
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
            jobs.WishListVMLikeJobType = new List<WishListVM>();
            foreach (var item in jobs.JobPostLikeJobTypes)
            {
                WishList wishListTemp = new WishList();

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
                jobs.WishListVMLikeJobType.Add(wishListVM);
            }
            return View("JobPostDetail", jobs);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        [Route("TimKiemViecLam")]
        public IActionResult SearchJob(string searchInHome, JobPostVM jobPostVM)
        {
            List<JobPost> jobPostExpireds;
            List<JobPost> jobPosts;
            if (searchInHome != null)
            {
                jobPosts = _unitOfWork.JobPost.GetAll(includeProperties: "Company").OrderByDescending(u => u.CreatedDate).Where(u => u.Heading.ToLower().Contains(searchInHome.ToLower()) || u.Company.Name.ToLower().Contains(searchInHome.ToLower()) && u.EndDate >= DateTime.Now).ToList();
                jobPostExpireds = _unitOfWork.JobPost.GetAll(includeProperties: "Company").OrderByDescending(u => u.CreatedDate).Where(u => u.Heading.ToLower().Contains(searchInHome.ToLower()) || u.Company.Name.ToLower().Contains(searchInHome.ToLower()) && u.EndDate < DateTime.Now).ToList();
                if (int.Parse(jobPostVM.SelectedJobType) != 0 && int.Parse(jobPostVM.SelectedCompany) != 0)
                {
                    jobPosts = jobPosts.Where(u => u.JobTypeId == int.Parse(jobPostVM.SelectedJobType.ToString()) && u.CompanyId == int.Parse(jobPostVM.SelectedCompany.ToString())).ToList();
                    jobPostExpireds = jobPostExpireds.Where(u => u.JobTypeId == int.Parse(jobPostVM.SelectedJobType.ToString()) && u.CompanyId == int.Parse(jobPostVM.SelectedCompany.ToString())).ToList();

                }
                else if (int.Parse(jobPostVM.SelectedCompany.ToString()) != 0)
                {
                    jobPosts = jobPosts.Where(u => u.CompanyId == int.Parse(jobPostVM.CompanyId.ToString())).ToList();
                    jobPostExpireds = jobPostExpireds.Where(u => u.CompanyId == int.Parse(jobPostVM.SelectedCompany.ToString())).ToList();
                }
                else if (int.Parse(jobPostVM.SelectedJobType.ToString()) != 0)
                {
                    jobPosts = jobPosts.Where(u => u.JobTypeId == int.Parse(jobPostVM.SelectedJobType.ToString())).ToList();
                    jobPostExpireds = jobPostExpireds.Where(u => u.JobTypeId == int.Parse(jobPostVM.SelectedJobType.ToString())).ToList();
                }
                else if (int.Parse(jobPostVM.SelectedCareer) != 0)
                {
                    var jobPostList = _unitOfWork.JobPost.GetAll(includeProperties: "Company,Job").OrderByDescending(u => u.CreatedDate).Where(u => u.EndDate >= DateTime.Now).ToList();
                    var jobPostExpiredList = _unitOfWork.JobPost.GetAll(includeProperties: "Company,Job").OrderByDescending(u => u.CreatedDate).Where(u => u.EndDate < DateTime.Now).ToList();
                    foreach (var jobPost in jobPostList)
                    {
                        if (jobPost.Job.CareerId.ToString() == jobPostVM.SelectedCareer)
                        {
                            jobPosts.Add(jobPost);
                        }
                    }
                    foreach (var jobPost in jobPostExpiredList)
                    {
                        if (jobPost.Job.CareerId.ToString() == jobPostVM.SelectedCareer)
                        {
                            jobPostExpireds.Add(jobPost);
                        }
                    }
                }
            }
            else
            {
                jobPosts = _unitOfWork.JobPost.GetAll(includeProperties: "Company").OrderByDescending(u => u.CreatedDate).Where(u => u.EndDate >= DateTime.Now).ToList();
                jobPostExpireds = _unitOfWork.JobPost.GetAll(includeProperties: "Company").OrderByDescending(u => u.CreatedDate).Where(u => u.EndDate < DateTime.Now).ToList();
                if (int.Parse(jobPostVM.SelectedJobType) != 0 && int.Parse(jobPostVM.SelectedCompany.ToString()) != 0)
                {
                    jobPosts = jobPosts.Where(u => u.JobTypeId == int.Parse(jobPostVM.SelectedJobType.ToString()) && u.CompanyId == int.Parse(jobPostVM.SelectedCompany.ToString())).ToList();
                    jobPostExpireds = jobPostExpireds.Where(u => u.JobTypeId == int.Parse(jobPostVM.SelectedJobType.ToString()) && u.CompanyId == int.Parse(jobPostVM.SelectedCompany.ToString())).ToList();
                }
                else if (int.Parse(jobPostVM.SelectedCompany.ToString()) != 0)
                {
                    jobPosts = jobPosts.Where(u => u.CompanyId == int.Parse(jobPostVM.SelectedCompany.ToString())).ToList();
                    jobPostExpireds = jobPostExpireds.Where(u => u.CompanyId == int.Parse(jobPostVM.SelectedCompany.ToString())).ToList();
                }
                else if (int.Parse(jobPostVM.SelectedJobType.ToString()) != 0)
                {
                    jobPosts = jobPosts.Where(u => u.JobTypeId == int.Parse(jobPostVM.SelectedJobType.ToString())).ToList();
                    jobPostExpireds = jobPostExpireds.Where(u => u.JobTypeId == int.Parse(jobPostVM.SelectedJobType.ToString())).ToList();
                }
                else if (int.Parse(jobPostVM.SelectedCareer.ToString()) != 0)
                {
                    var jobPostList = _unitOfWork.JobPost.GetAll(includeProperties: "Company,Job").OrderByDescending(u => u.CreatedDate).Where(u => u.EndDate >= DateTime.Now).ToList();
                    var jobPostExpiredList = _unitOfWork.JobPost.GetAll(includeProperties: "Company,Job").OrderByDescending(u => u.CreatedDate).Where(u => u.EndDate < DateTime.Now).ToList();
                    var jobPostTemp = new List<JobPost>();
                    var jobPostExpiredTemp = new List<JobPost>();
                    foreach (var jobPost in jobPostList)
                    {
                        if (jobPost.Job.CareerId.ToString() == jobPostVM.SelectedCareer.ToString())
                        {
                            jobPostTemp.Add(jobPost);
                        }
                    }
                    foreach (var jobPost in jobPostExpiredList)
                    {
                        if (jobPost.Job.CareerId.ToString() == jobPostVM.SelectedCareer.ToString())
                        {
                            jobPostExpiredTemp.Add(jobPost);
                        }
                    }
                    jobPosts = jobPostTemp;
                    jobPostExpireds = jobPostExpiredTemp;
                }
                else
                {
                    jobPosts = jobPosts;
                    jobPostExpireds = jobPostExpireds;
                }

            }
            var wishList = _unitOfWork.WishList.GetAll().ToList();
            JobPostVM jobs = new()
            {
                JobPosts = jobPosts,
                JobPostExpireds = jobPostExpireds,
                Companies = _unitOfWork.Company.GetAll().ToList(),
                listTag = _unitOfWork.Tag.GetAll(includeProperties: "JobPosts,Companies").ToList(),
                JobTypeList = _unitOfWork.JobType.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                CompanyList = _unitOfWork.Company.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                CareerList = _unitOfWork.Career.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                SelectedCareer = jobPostVM.SelectedCareer,
                SelectedCompany = jobPostVM.SelectedCompany,
                SelectedJobType = jobPostVM.SelectedJobType,
                JobTypes = _unitOfWork.JobType.GetAll().ToList(),
                CompanyImages = _unitOfWork.CompanyImage.GetAll().ToList(),
                WistList = wishList,
            };
            jobs.WishListVM = new List<WishListVM>();
            jobs.WishListVMExprireds = new List<WishListVM>();
            WishListVM wishListVM;
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
            foreach (var item in jobs.JobPostExpireds)
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
                jobs.WishListVMExprireds.Add(wishListVM);
            }
            return View(jobs);
        }
        [Route("/SearchByTag/{tag}")]
        public IActionResult SearchByTag(string tag)
        {
            var objTag = _unitOfWork.Tag.Get(filter: u => u.Name.Contains(tag) == true, includeProperties: "JobPosts,Companies");
            var jobPosts = new List<JobPost>();
            var jobPostExpireds = new List<JobPost>();
            foreach (var item in objTag.JobPosts)
            {
                if (item.EndDate < DateTime.Now)
                {
                    jobPostExpireds.Add(item);
                }
                else
                {
                    jobPosts.Add(item);
                }
            }
            var wishList = _unitOfWork.WishList.GetAll().ToList();
            JobPostVM jobs = new()
            {
                JobPosts = jobPosts,
                JobPostExpireds = jobPostExpireds,
                Companies = _unitOfWork.Company.GetAll().ToList(),
                listTag = _unitOfWork.Tag.GetAll(includeProperties: "JobPosts,Companies").ToList(),
                JobTypeList = _unitOfWork.JobType.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                CompanyList = _unitOfWork.Company.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                CareerList = _unitOfWork.Career.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                JobTypes = _unitOfWork.JobType.GetAll().ToList(),
                CompanyImages = _unitOfWork.CompanyImage.GetAll().ToList(),
                WistList = wishList,
            };
            jobs.WishListVM = new List<WishListVM>();
            jobs.WishListVMExprireds = new List<WishListVM>();
            WishListVM wishListVM;
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
            foreach (var item in jobs.JobPostExpireds)
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
                jobs.WishListVMExprireds.Add(wishListVM);
            }
            return View(jobs);
        }
        [Route("/chat")]
        [Authorize]
        public IActionResult Chat()
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