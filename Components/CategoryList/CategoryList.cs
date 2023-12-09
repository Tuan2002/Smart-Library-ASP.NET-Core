using Microsoft.AspNetCore.Mvc;
using Smart_Library.Models;
using Smart_Library.Data;
using Smart_Library.Services;

namespace Smart_Library.Areas.Admin.Components
{
    [ViewComponent(Name = "CategoryList")]
    public class CategoryList : ViewComponent
    {
        private readonly ApplicationDBContext _context;
        private readonly IBooksService _booksService;
        public CategoryList(ApplicationDBContext context, IBooksService booksService)
        {
            _context = context;
            _booksService = booksService;
        }
        public async Task<IViewComponentResult> InvokeAsync(int? selected)
        {
            var response = await _booksService.GetCategoriesAsync();
            var categories = response.Data as List<CategoryViewModel>;
            return await Task.FromResult((IViewComponentResult)View(categories));
        }
    }
}