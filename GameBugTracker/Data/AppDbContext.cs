using GameBugTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace GameBugTracker.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<BugReport> BugReports => Set<BugReport>();
}
