using System.ComponentModel.DataAnnotations;

namespace GameBugTracker.Models;

public class BugReport
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = "";

    [Required]
    [StringLength(2000)]
    public string Description { get; set; } = "";

    public BugSeverity Severity { get; set; } = BugSeverity.Medium;

    public BugStatus Status { get; set; } = BugStatus.Open;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
