using Microsoft.AspNetCore.Mvc;

namespace Smart_Library.Controllers
{
    [Route("[controller]")]
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return Redirect("/");
        }
        [Route("Locked")]
        public IActionResult Locked()
        {
            return View();
        }
        [Route("{code}")]
        public IActionResult Error(int code)
        {
            return code switch
            {
                401 => RedirectToAction("Login", "Account"),
                403 => View("Forbidden"),
                404 => View("NotFound"),
                500 => View("InternalServerError"),
                _ => View("InternalServerError"),
            };
        }
    }
}