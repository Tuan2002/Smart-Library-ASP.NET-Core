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
    }
}