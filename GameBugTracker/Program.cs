using GameBugTracker.Configuration;
using GameBugTracker.Data;
using GameBugTracker.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace GameBugTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

            // Add services to the container.
            builder.Services.Configure<DemoAdminOptions>(
                builder.Configuration.GetSection(DemoAdminOptions.SectionName));
            builder.Services.Configure<SiteOwnerOptions>(
                builder.Configuration.GetSection(SiteOwnerOptions.SectionName));

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.Cookie.Name = "GameBugTracker.Auth";
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.LoginPath = "/Auth/Login";
                    options.AccessDeniedPath = "/Auth/Login";
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromHours(8);
                });

            builder.Services.AddAuthorization();
            builder.Services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizePage("/BugReports/Create");
                options.Conventions.AuthorizePage("/BugReports/Edit");
                options.Conventions.AuthorizePage("/Auth/Logout");
                options.Conventions.AllowAnonymousToPage("/Auth/Login");
            });
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();

                if (!dbContext.BugReports.Any())
                {
                    dbContext.BugReports.AddRange(
                        new BugReport
                        {
                            Title = "Player falls through the floor in the cargo bay",
                            Description = "When the player jumps near the back-left corner of the cargo bay, the character clips through the floor and cannot recover.",
                            Severity = BugSeverity.High,
                            Status = BugStatus.Open
                        },
                        new BugReport
                        {
                            Title = "Pause menu does not open with controller input",
                            Description = "The pause menu opens with the keyboard, but pressing Start on the controller does nothing during gameplay.",
                            Severity = BugSeverity.Medium,
                            Status = BugStatus.InProgress
                        },
                        new BugReport
                        {
                            Title = "Mission complete screen overlaps subtitles",
                            Description = "At the end of the first mission, the completion banner covers subtitle text for several seconds.",
                            Severity = BugSeverity.Low,
                            Status = BugStatus.Fixed
                        });

                    dbContext.SaveChanges();
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
