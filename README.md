# WaterTrackerApp

Blazor WebAssembly application with a .NET 8 REST API backend for tracking and managing users water consumption.

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) 

- ## Features
- **User Management:**  
  - Add, edit, and delete users
  - View all users in a table

- **Water Intake Management:**  
  - Add, edit, and delete water intake records for each user
  - View water consumption history per user

### Startup projects
- API > Client  

### Apply migrations and update the database, run:
- dotnet ef database update --project WaterTrackerApp.Infrastructure

  **Note:**  
> Make sure your default project is `WaterTrackerApp.Infrastructure` when running EF Core commands.
