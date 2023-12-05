using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Smart_Library.Areas.Admin.Models;
using Smart_Library.Data;
using Smart_Library.Entities;
using Smart_Library.Utils;

namespace Smart_Library.Areas.Admin.Services
{
    public interface IBooksManagerService
    {

    }
    public class BooksManagerService : IBooksManagerService
    {
        public readonly ILogger<BooksManagerService> _logger;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDBContext _context;

        public BooksManagerService(ILogger<BooksManagerService> logger, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, ApplicationDBContext context)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _context = context;
        }

    }
}