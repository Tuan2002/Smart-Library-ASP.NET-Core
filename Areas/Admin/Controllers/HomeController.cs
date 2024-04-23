using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Smart_Library.Config;

namespace Smart_Library.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppRoles.Admin)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

    }
}