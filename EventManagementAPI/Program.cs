using Microsoft.EntityFrameworkCore;
using EventManagementAPI.Infrastructure.Data;
using EventManagementAPI.Application.Interfaces;
using EventManagementAPI.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure EF Core to use SQL Server
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register application services
builder.Services.AddScoped<IEventService, EventService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
