using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using ProductApi.MinimalApi.Data;
using ProductApi.MinimalApi.Services;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddFastEndpoints();

var app = builder.Build();
app.UseFastEndpoints();
app.MapGet("/", () => "Hello World!");

app.Run();
