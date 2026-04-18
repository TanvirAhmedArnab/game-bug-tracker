namespace GameBugTracker.Models;

public class BugReport
{
    public int Id { get; set; }

    public string Title { get; set; } = "";

    public string Description { get; set; } = "";

    public BugSeverity Severity { get; set; } = BugSeverity.Medium;

    public BugStatus Status { get; set; } = BugStatus.Open;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
