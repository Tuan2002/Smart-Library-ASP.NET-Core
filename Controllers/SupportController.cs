using Microsoft.AspNetCore.Mvc;
using Smart_Library.Data;
using Smart_Library.Entities;
using Smart_Library.Models;

namespace Smart_Library.Controllers
{
    [Route("[controller]")]
    public class SupportController : Controller
    {
        private readonly ILogger<SupportController> _logger;
        private readonly ApplicationDBContext _context;

        public SupportController(ILogger<SupportController> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("Create/Ticket")]
        public IActionResult CreateTicket()
        {
            return View();
        }
        [HttpPost]
        [Route("Create/Ticket")]
        public async Task<IActionResult> CreateTicket(CreateTicketModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Status"] = "error";
                return RedirectToAction("CreateResult");
            }
            var newTicket = new SupportTicket
            {
                Title = model.Title,
                FullName = model.FullName,
                Phone = model.Phone,
                RequestDescription = model.RequestDescription,
                StudyMajor = model.StudyMajor,
                StudyGrade = model.StudyGrade,
                EnglishLevel = model.EnglishLevel,
                SupportTypeId = model.SupportTypeId,
                CreatedAt = DateTime.Now
            };
            var Result = await _context.SupportTickets.AddAsync(newTicket);
            var Ticket = Result.Entity as SupportTicket;
            var IsSuccess = await _context.SaveChangesAsync();
            if (IsSuccess == 0)
            {
                TempData["Status"] = "error";
                return RedirectToAction("CreateResult");
            }
            TempData["Status"] = "success";
            return RedirectToAction("CreateResult", new { id = Ticket.Id });

        }
        [HttpGet]
        [Route("Create/Result")]
        public IActionResult CreateResult(string? id)
        {
            if (id == null)
            {
                return View();
            }
            var ticketId = int.Parse(id);
            var ticket = _context.SupportTickets.Where(t => t.Id == ticketId).FirstOrDefault();
            if (ticket == null)
            {
                return View();
            }
            var ticketInfo = new CreateTicketModel
            {
                Title = ticket.Title,
                FullName = ticket.FullName,
                Phone = ticket.Phone,
                RequestDescription = ticket.RequestDescription,
                StudyMajor = ticket.StudyMajor,
                StudyGrade = ticket.StudyGrade,
                EnglishLevel = ticket.EnglishLevel,
                SupportTypeId = ticket.SupportTypeId
            };
            return View(ticketInfo);
        }

    }
}