// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TuyenDungWeb.Utility;
using TuyenDungWeb.Views.Shared.Components.MessagePage;

namespace TuyenDungWeb.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<IdentityUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Không để trống")]
            [Display(Name = "Nhập username hoặc email của bạn")]
            [StringLength(100, MinimumLength = 1, ErrorMessage = "Nhập đúng thông tin")]
            public string UserNameOrEmail { set; get; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu")]
            public string Password { get; set; }

            [Display(Name = "Nhớ thông tin đăng nhập?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Đã đăng nhập nên chuyển hướng về Index
            if (_signInManager.IsSignedIn(User)) return Redirect("Index");

            if (ModelState.IsValid)
            {
                // Thử login bằng username/password
                var result = await _signInManager.PasswordSignInAsync(
                    Input.UserNameOrEmail,
                    Input.Password,
                    Input.RememberMe,
                    true
                );

                if (!result.Succeeded)
                {
                    // Thất bại username/password -> tìm user theo email, nếu thấy thì thử đăng nhập
                    // bằng user tìm được
                    var user = await _userManager.FindByEmailAsync(Input.UserNameOrEmail);
                    if (user != null)
                    {
                        result = await _signInManager.PasswordSignInAsync(
                            user,
                            Input.Password,
                            Input.RememberMe,
                            true
                        );
                    }
                }

                if (result.Succeeded)
                {
                    _logger.LogInformation("User đã đăng nhập");
                    //if admin
                    if (User.IsInRole(SD.Role_Admin))
                    {
                        return ViewComponent(MessagePage.COMPONENTNAME, new MessagePage.Message()
                        {
                            title = "Đã đăng nhập",
                            htmlcontent = "Đăng nhập thành công",
                            urlredirect = "/Admin/Home/Index"
                        });
                    }
                    //if employer
                    if (User.IsInRole("Employer"))
                    {
                        return Redirect("/Employer/Home/Index");
                    }

                    return ViewComponent(MessagePage.COMPONENTNAME, new MessagePage.Message()
                    {
                        title = "Đã đăng nhập",
                        htmlcontent = "Đăng nhập thành công",
                        urlredirect = returnUrl
                    });

                }
                if (result.RequiresTwoFactor)
                {
                    // Nếu cấu hình đăng nhập hai yếu tố thì chuyển hướng đến LoginWith2fa
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Tài khoản bí tạm khóa.");
                    // Chuyển hướng đến trang Lockout - hiện thị thông báo
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Không đăng nhập được.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
