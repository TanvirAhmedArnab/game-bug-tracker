using GameBugTracker.Data;
using GameBugTracker.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameBugTracker.Pages.BugReports
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<BugReport> BugReports { get; private set; } = new List<BugReport>();

        public async Task OnGetAsync()
        {
            BugReports = await _context.BugReports
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }
    }
}
