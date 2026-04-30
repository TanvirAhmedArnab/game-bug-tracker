namespace GameBugTracker.Configuration;

public class DemoAdminOptions
{
    public const string SectionName = "DemoAdmin";

    public string UserName { get; set; } = "owner";

    public string Password { get; set; } = "";
}
