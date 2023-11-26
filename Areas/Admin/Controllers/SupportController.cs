using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Smart_Library.Areas.Admin.Models;
using Smart_Library.Data;

namespace Smart_Library.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class SupportController : Controller
    {
        private readonly ILogger<SupportController> _logger;
        private readonly ApplicationDBContext _context;

        public SupportController(ILogger<SupportController> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var TicketList = await _context.SupportTickets.Select(ticket => new SupportTicketViewModel
            {
                Id = ticket.Id,
                Title = ticket.Title,
                FullName = ticket.FullName,
                Phone = ticket.Phone,
                RequestDescription = ticket.RequestDescription,
                StudyMajor = ticket.StudyMajor,
                StudyGrade = ticket.StudyGrade,
                EnglishLevel = ticket.EnglishLevel,
                SupportType = ticket.SupportType.Name,
                Status = ticket.Status,
                CreatedAt = ticket.CreatedAt
            }).ToListAsync();
            return View(TicketList);
        }
        [HttpGet("{id:int}/view")]
        public async Task<IActionResult> TicketDetail(int? id)
        {
            var TicketInfo = await _context.SupportTickets.Where(ticket => ticket.Id == id).Select(ticket => new SupportTicketViewModel
            {
                Id = ticket.Id,
                Title = ticket.Title,
                FullName = ticket.FullName,
                Phone = ticket.Phone,
                RequestDescription = ticket.RequestDescription,
                StudyMajor = ticket.StudyMajor,
                StudyGrade = ticket.StudyGrade,
                EnglishLevel = ticket.EnglishLevel,
                SupportType = ticket.SupportType.Name,
                Status = ticket.Status,
                CreatedAt = ticket.CreatedAt
            }).FirstOrDefaultAsync();
            if (TicketInfo == null)
            {
                return NotFound();
            }
            return View(TicketInfo);
        }
        [HttpPost("CompleteTask")]
        public async Task<IActionResult> CompleteTask(int? id)
        {
            var Ticket = await _context.SupportTickets.FindAsync(id);
            if (Ticket == null)
            {
                TempData["SupportMessage"] = "Không tìm thấy yêu cầu hỗ trợ";
                TempData["Type"] = "error";
                return RedirectToAction("Index");

            }
            Ticket.Status = true;
            _context.Update(Ticket);
            var IsSuccess = await _context.SaveChangesAsync();
            if (IsSuccess == 0)
            {
                TempData["SupportMessage"] = "Có lỗi xảy ra, vui lòng thử lại sau";
                TempData["Type"] = "error";
                return RedirectToAction("Index");
            }
            TempData["SupportMessage"] = "Đã hoàn thành yêu cầu hỗ trợ";
            TempData["Type"] = "success";
            return RedirectToAction("Index");
        }
        [HttpPost("DeleteTask")]
        public async Task<IActionResult> DeleteTask(int? id)
        {
            try
            {

                var Ticket = await _context.SupportTickets.FindAsync(id);
                if (Ticket == null)
                {
                    TempData["SupportMessage"] = "Không tìm thấy yêu cầu hỗ trợ";
                    TempData["Type"] = "error";
                    return RedirectToAction("Index");

                }
                _context.Remove(Ticket);
                await _context.SaveChangesAsync();
                TempData["SupportMessage"] = "Đã xoá yêu cầu hỗ trợ";
                TempData["Type"] = "success";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                TempData["SupportMessage"] = "Có lỗi xảy ra, vui lòng thử lại sau";
                TempData["Type"] = "error";
                return RedirectToAction("Index");
            }
        }
    }
}