using WaterTrackerApp.Client;
using WaterTrackerApp.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7279/") }); //use base api url
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<WaterIntakeService>();

await builder.Build().RunAsync();
