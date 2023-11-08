using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Smart_Library.Entities;
using Smart_Library.Models;

namespace Smart_Library.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(ILogger<AccountController> logger, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return Redirect("/");
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Logout")]
        public async Task<IActionResult> Logout(string returnUrl = null!)
        {
            await _signInManager.SignOutAsync();
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
            try
            {
                var user = await _userManager.FindByEmailAsync(loginModel.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Thông tin đăng nhập không chính xác");
                    return View(loginModel);
                }
                var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: true);
                if (!result.Succeeded)
                {
                    if (result.IsLockedOut)
                    {
                        ModelState.AddModelError(string.Empty, "Tài khoản của bạn đã bị khóa do đăng nhập sai quá nhiều lần");
                        return View(loginModel);
                    }
                    ModelState.AddModelError(string.Empty, "Thông tin đăng nhập không chính xác");
                    await _userManager.AccessFailedAsync(user);
                    return View(loginModel);
                }
                TempData["Message"] = "Đăng nhập thành công";
                TempData["Type"] = "success";
                var roles = await _userManager.GetRolesAsync(user);
                if (!string.IsNullOrEmpty(loginModel.ReturnUrl) && Url.IsLocalUrl(loginModel.ReturnUrl))
                {
                    if (!roles.Contains("Quản trị viên") && loginModel.ReturnUrl.Contains("/admin"))
                        return RedirectToAction("Index", "Home");
                    return Redirect(loginModel.ReturnUrl);
                }
                if (roles.Contains("Quản trị viên"))
                    return Redirect("/admin");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error at: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                ModelState.AddModelError(string.Empty, "Lỗi hệ thống, vui lòng thử lại sau!");
                return View(loginModel);
            }
        }
    }
}