[![Build](https://github.com/TanvirAhmedArnab/game-bug-tracker/actions/workflows/dotnet-build.yml/badge.svg)](https://github.com/TanvirAhmedArnab/game-bug-tracker/actions/workflows/dotnet-build.yml)

# Game Bug Tracker

Game Bug Tracker is a portfolio project by **Tanvir Ahmed Arnab**. It is a small ASP.NET Core Razor Pages app for tracking bugs on a game project, with EF Core and SQLite handling persistence.

The app is designed to be safe to show publicly:

- anyone can browse the tracker
- only the owner account can create or edit bug reports
- secrets stay out of the repository

## Tech Stack

- C#
- .NET 10
- ASP.NET Core Razor Pages
- Entity Framework Core
- SQLite
- Bootstrap

## What Version 1 Includes

- public home page
- public bug list
- public bug details page
- owner-only create bug page
- owner-only edit bug page
- validation for bug titles and descriptions
- EF Core migrations
- automatic demo-data seeding for an empty database

## Security Model

This repository is public on purpose so recruiters and interviewers can review the code.

To keep the live app safe:

- the owner username is configurable
- the owner password is **not** stored in source control
- the password must be provided through environment configuration
- anonymous visitors can read data, but cannot modify it

For a production deployment, the most important environment variable is:

```powershell
$env:DemoAdmin__Password = "choose-a-strong-password"
```

You can optionally change the owner username too:

```powershell
$env:DemoAdmin__UserName = "owner"
```

## Run Locally

### Visual Studio Community

1. Open `C:\Users\taarn\Desktop\.NET Portfolio\GameBugTracker`
2. Set an owner password in a terminal before launch:

```powershell
$env:DemoAdmin__Password = "choose-a-strong-password"
```

3. Run the app with `Ctrl + F5`

### .NET CLI

```powershell
cd "C:\Users\taarn\Desktop\.NET Portfolio\GameBugTracker"
$env:DemoAdmin__Password = "choose-a-strong-password"
dotnet build
dotnet run
```

## Deployment Notes

The app is ready for a public single-owner demo deployment, but the actual hosting step still depends on your hosting account.

Before deploying:

1. set `DemoAdmin__Password`
2. keep `ASPNETCORE_ENVIRONMENT` set to `Production`
3. configure a production connection string if you do not want to keep using local SQLite storage

For a small demo, SQLite is fine on a single instance with persistent storage. For a more serious public deployment, move to a hosted database such as SQL Server or PostgreSQL.

## Why The Repo Can Stay Public

Public source code does **not** mean public admin access.

This works because:

- app code is public
- admin credentials are private
- production secrets come from the host environment, not from the repo

## Author

Built by **Tanvir Ahmed Arnab**

- GitHub: [TanvirAhmedArnab](https://github.com/TanvirAhmedArnab)
