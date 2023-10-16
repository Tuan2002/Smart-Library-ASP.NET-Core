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
        [Route("NotFound")]
        public new IActionResult NotFound()
        {
            return View("NotFound");
        }
        [Route("InternalServerError")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult InternalServerError()
        {
            return View("InternalServerError");
        }
        [Route("Forbidden")]
        public IActionResult Forbidden()
        {
            return View("Forbidden");
        }

    }
}