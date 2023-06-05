// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TuyenDungWeb.DataAccess.Data;
using TuyenDungWeb.DataAccess.Repositories.IRepository;

namespace TuyenDungWeb.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment webHostEnvironment,
            ApplicationDbContext context
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "SĐT")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Ảnh đại diện")]
            public string Avatar { get; set; }
            [Required(ErrorMessage = "Vui lòng nhập họ tên")]
            [Display(Name = "Họ tên")]
            public string FullName { get; set; }

            [Display(Name = "Địa chỉ")]
            public string Address { get; set; }
            [Display(Name = "Vai trò")]
            public string Role { get; set; }
            [Display(Name = "Công ty")]
            public string Company { get; set; }

        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var avatar = _unitOfWork.ApplicationUser.Get(filter: u => u.Id == user.Id, includeProperties: "Company").Avatar;
            var fullName = _unitOfWork.ApplicationUser.Get(filter: u => u.Id == user.Id, includeProperties: "Company").FullName;
            var address = _unitOfWork.ApplicationUser.Get(filter: u => u.Id == user.Id, includeProperties: "Company").Address;
            var role = _userManager.GetRolesAsync(_unitOfWork.ApplicationUser.Get(u => u.Id == user.Id))
                    .GetAwaiter().GetResult().FirstOrDefault();
            var company = _unitOfWork.ApplicationUser.Get(filter: u => u.Id == user.Id, includeProperties: "Company").Company;
            Username = userName;

            Input = new InputModel();
            if (company != null)
            {
                Input.Company = company.Name;
            }
            if (role != null)
            {
                Input.Role = role;
            }
            if (avatar != null)
            {
                Input.Avatar = avatar;
            }
            if (fullName != null)
            {
                Input.FullName = fullName;
            }
            if (address != null)
            {
                Input.Address = address;
            }
            Input.PhoneNumber = phoneNumber;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var avatar = _unitOfWork.ApplicationUser.Get(filter: u => u.Id == user.Id, includeProperties: "Company").Avatar;
            var fullName = _unitOfWork.ApplicationUser.Get(filter: u => u.Id == user.Id, includeProperties: "Company").FullName;
            var address = _unitOfWork.ApplicationUser.Get(filter: u => u.Id == user.Id, includeProperties: "Company").Address;
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Lỗi trong khi cập nhật số điện thoại.";
                    return RedirectToPage();
                }
            }
            if (Input.Address != address || Input.FullName != fullName || file != null)
            {
                var appUser = _context.ApplicationUsers.Where(u => u.Id == user.Id).FirstOrDefault();
                if (Input.Address != null)
                {
                    appUser.Address = Input.Address;
                }
                if (Input.FullName != null)
                {
                    appUser.FullName = Input.FullName;
                }

                if (file != null && file.Length > 0)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string avatarPath = @"images\avatars\";
                    string finalPath = Path.Combine(wwwRootPath, avatarPath);

                    if (!Directory.Exists(finalPath))
                        Directory.CreateDirectory(finalPath);

                    using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    appUser.Avatar = @"\" + avatarPath + @"\" + fileName;
                }
                await _context.SaveChangesAsync();
            }
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Hồ sơ đã được cập nhật";
            return RedirectToPage();
        }
    }
}
