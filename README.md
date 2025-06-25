# WaterTrackerApp

Blazor WebAssembly application with a .NET 8 REST API backend for tracking and managing users water consumption.

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) 

### Startup projects
- API > Client  

### Apply migrations and update the database from WaterTrackerApp.Infrastructure, run:
- dotnet ef database update --project WaterTrackerApp.Infrastructure

### Future Improvements:
- Implement refit to simplify and reduce boilerplate and make for easier testing
 - More input validation and error handling 
 - UI/UX - Make more responsive and accessible
 - For data changes such as TotalConsumed - could have used SignalR or shared service to keep synced and updated and reduce redundant api calls
