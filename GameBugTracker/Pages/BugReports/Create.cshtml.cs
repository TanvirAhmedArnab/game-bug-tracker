using GameBugTracker.Data;
using GameBugTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameBugTracker.Pages.BugReports
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BugReport BugReport { get; set; } = new BugReport();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BugReports.Add(BugReport);
            await _context.SaveChangesAsync();

            return RedirectToPage("/BugReports/Index");
        }
    }
}
