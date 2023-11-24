using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Smart_Library.Controllers
{
    [Route("[controller]")]
    public class SupportController : Controller
    {
        private readonly ILogger<SupportController> _logger;

        public SupportController(ILogger<SupportController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

    }
}