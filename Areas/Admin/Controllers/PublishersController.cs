using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_Library.Areas.Admin.Models;
using Smart_Library.Areas.Admin.Services;
using Smart_Library.Config;
using Smart_Library.Models;

namespace Smart_Library.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    [Authorize(Roles = AppRoles.Admin)]
    public class PublishersController : Controller
    {
        private readonly ILogger<PublishersController> _logger;
        private readonly IPublishManagerService _publishManagerService;

        public PublishersController(ILogger<PublishersController> logger, IPublishManagerService publishManagerService)
        {
            _logger = logger;
            _publishManagerService = publishManagerService;
        }

        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            var response = await _publishManagerService.GetPublishersAsync(page, pageSize);
            if (!response.IsSuccess)
            {
                return StatusCode(500);
            }
            var data = response.Data as dynamic;
            ViewBag.TotalPublishers = data?.totalPublishers;
            ViewBag.TotalPages = data?.totalPages;
            ViewBag.CurrentPage = data?.currentPage;
            ViewBag.CurrentPageSize = data?.currentPageSize;
            return View(data?.publishers as List<PublisherViewModel>);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create/Publisher")]
        public async Task<IActionResult> CreatePublisher(CreatePublisherModel newPublisher)
        {
            var response = await _publishManagerService.CreatePublisherAsync(newPublisher);
            if (!response.IsSuccess)
            {
                TempData["SystemMessage"] = response.Message;
                TempData["Type"] = "error";
                return RedirectToAction("Index");
            }
            TempData["SystemMessage"] = response.Message;
            TempData["Type"] = "success";
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Update/Publisher")]
        public async Task<IActionResult> UpdatePublisher(UpdatePublisherModel updatePublisher)
        {
            var response = await _publishManagerService.UpdatePublisherAsync(updatePublisher);
            if (!response.IsSuccess)
            {
                TempData["SystemMessage"] = response.Message;
                TempData["Type"] = "error";
                return RedirectToAction("Index");
            }
            TempData["SystemMessage"] = response.Message;
            TempData["Type"] = "success";
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Delete/Publisher")]
        public async Task<IActionResult> DeletePublisher(int publisherId)
        {
            var response = await _publishManagerService.DeletePublisherAsync(publisherId);
            if (!response.IsSuccess)
            {
                TempData["SystemMessage"] = response.Message;
                TempData["Type"] = "error";
                return RedirectToAction("Index");
            }
            TempData["SystemMessage"] = response.Message;
            TempData["Type"] = "success";
            return RedirectToAction("Index");
        }
    }
}