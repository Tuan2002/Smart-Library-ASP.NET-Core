
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Library.Data;

namespace Smart_Library.Components
{
    [ViewComponent(Name = "SupportTypeOptions")]
    public class SupportTypeOptions : ViewComponent
    {
        private readonly ApplicationDBContext _context;

        public SupportTypeOptions(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var supportTypes = await _context.SupportTypes.ToListAsync();
            return await Task.FromResult((IViewComponentResult)View("SupportTypeOptions", supportTypes));
        }

    }
}