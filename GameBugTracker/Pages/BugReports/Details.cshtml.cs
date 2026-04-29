using GameBugTracker.Data;
using GameBugTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameBugTracker.Pages.BugReports
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;

        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }

        public BugReport? BugReport { get; private set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            BugReport = await _context.BugReports.FirstOrDefaultAsync(b => b.Id == id);

            if (BugReport == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
