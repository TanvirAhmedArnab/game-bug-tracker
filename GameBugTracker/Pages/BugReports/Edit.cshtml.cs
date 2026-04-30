using GameBugTracker.Data;
using GameBugTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameBugTracker.Pages.BugReports
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BugReport BugReport { get; set; } = new BugReport();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var existingBug = await _context.BugReports.FirstOrDefaultAsync(b => b.Id == id);

            if (existingBug == null)
            {
                return NotFound();
            }

            BugReport = existingBug;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingBug = await _context.BugReports.FirstOrDefaultAsync(b => b.Id == BugReport.Id);

            if (existingBug == null)
            {
                return NotFound();
            }

            existingBug.Title = BugReport.Title;
            existingBug.Description = BugReport.Description;
            existingBug.Severity = BugReport.Severity;
            existingBug.Status = BugReport.Status;

            await _context.SaveChangesAsync();

            return RedirectToPage("/BugReports/Details", new { id = existingBug.Id });
        }
    }
}
