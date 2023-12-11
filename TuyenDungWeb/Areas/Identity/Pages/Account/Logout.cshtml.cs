using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TuyenDungWeb.Views.Shared.Components.MessagePage;

[AllowAnonymous]
public class LogoutModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ILogger<LogoutModel> _logger;

    public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<IActionResult> OnPost(string returnUrl = null)
    {
        if (!_signInManager.IsSignedIn(User)) return RedirectToPage("/Index");

        await _signInManager.SignOutAsync();
        _logger.LogInformation("Người dùng đăng xuất");


        return ViewComponent(MessagePage.COMPONENTNAME,
            new MessagePage.Message()
            {
                title = "Đã đăng xuất",
                htmlcontent = "Đăng xuất thành công",
                urlredirect = (returnUrl != null) ? returnUrl : Url.Page("/Index")
            }
        );
    }
}