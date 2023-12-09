using Microsoft.AspNetCore.Mvc;
using Smart_Library.Models;
using Smart_Library.Data;
using Smart_Library.Services;

namespace Smart_Library.Areas.Admin.Components
{
    [ViewComponent(Name = "AuthorList")]
    public class AuthorList : ViewComponent
    {
        private readonly ApplicationDBContext _context;
        private readonly IBooksService _booksService;
        public AuthorList(ApplicationDBContext context, IBooksService booksService)
        {
            _context = context;
            _booksService = booksService;
        }
        public async Task<IViewComponentResult> InvokeAsync(int? selected)
        {
            var response = await _booksService.GetAuthorsAsync();
            var categories = response.Data as List<AuthorViewModel>;
            return await Task.FromResult((IViewComponentResult)View(categories));
        }
    }
}