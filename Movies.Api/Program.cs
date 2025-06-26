using Movies.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplication();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();