using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Library.Areas.Admin.Models;
using Smart_Library.Data;

namespace Smart_Library.Areas.Admin.Components
{
    [ViewComponent(Name = "WorkspaceOptions")]
    public class WorkspaceList : ViewComponent
    {
        private readonly ApplicationDBContext _context;

        public WorkspaceList(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var workspaces = await (
                from workspace in _context.Workspace
                select new WorkspaceViewModel
                {
                    WorkspaceId = workspace.WorkspaceId,
                    WorkspaceName = workspace.WorkspaceName,
                    CreatedAt = workspace.CreatedAt
                }).ToListAsync();
            return await Task.FromResult((IViewComponentResult)View("WorkspaceOptions", workspaces));
        }

    }
}