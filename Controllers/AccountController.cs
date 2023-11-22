using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_Library.Models;
using Smart_Library.Services;

namespace Smart_Library.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;
        public AccountController(ILogger<AccountController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }
        public IActionResult Index()
        {
            return NotFound();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Logout")]
        public async Task<IActionResult> Logout(string returnUrl = null!)
        {
            var LogoutResult = await _accountService.LogoutAsync();
            if (!LogoutResult.IsSuccess)
            {
                TempData["AuthMessage"] = LogoutResult?.ErrorMessage ?? "Đăng xuất thất bại";
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "Account", new { returnUrl });
        }
        [HttpGet]
        [Route("Login")]
        public IActionResult Login(string returnUrl)
        {
            // Check if user is already logged in
            if (User?.Identity?.IsAuthenticated == true)
            {
                return Redirect(returnUrl ?? "/");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginModel { ReturnUrl = returnUrl });
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var LoginResult = await _accountService.LoginAsync(loginModel);
            if (!LoginResult.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, LoginResult?.ErrorMessage ?? "Đăng nhập thất bại");
                return View(loginModel);
            }
            TempData["AuthMessage"] = "Đăng nhập thành công";
            TempData["Type"] = "success";
            if (LoginResult.Roles != null && LoginResult.Roles.Contains("Quản trị viên") && loginModel.ReturnUrl == "/")
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            if (!string.IsNullOrEmpty(loginModel.ReturnUrl) && Url.IsLocalUrl(loginModel.ReturnUrl))
            {
                if (LoginResult.Roles != null && !LoginResult.Roles.Contains("Quản trị viên") && loginModel.ReturnUrl.Contains("/admin"))
                    return RedirectToAction("Index", "Home");
                return Redirect(loginModel.ReturnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}