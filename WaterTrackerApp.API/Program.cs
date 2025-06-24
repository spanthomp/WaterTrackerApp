using WaterTrackerApp.Application.Interfaces;
using WaterTrackerApp.Infrastructure.Services;
using WaterTrackerApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext with sql server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//register services
builder.Services.AddScoped<IWaterIntakeService, WaterIntakeService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<WaterIntakeService>();

builder.Services.AddCors(options =>
   {
       options.AddPolicy("AllowBlazorClient",
           policy => policy
               .WithOrigins("https://localhost:7247") 
               .AllowAnyHeader()
               .AllowAnyMethod());
   });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowBlazorClient");

app.UseAuthorization();

app.MapControllers();

app.Run();
