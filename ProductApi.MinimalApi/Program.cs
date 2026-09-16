using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.MinimalApi.Data;
using ProductApi.MinimalApi.DTOs;
using ProductApi.MinimalApi.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IProductService, ProductService>();
static string? ValidateProductRequest(CreateProductRequest request)
{
    if(string.IsNullOrWhiteSpace(request.Name))
        return "Product name cannot be empty.";
    if(request.Price <= 0)
        return "Product price must be a positive value.";
    return null;
}

var app = builder.Build();
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        context.Response.ContentType =
            "application/json";

        var exceptionFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();

        var exception =
            exceptionFeature?.Error;

        var logger =
            context.RequestServices
                .GetRequiredService<ILogger<Program>>();

        logger.LogError(
            exception,
            "Unhandled exception occurred");

        await context.Response.WriteAsJsonAsync(new
        {
            error = "An unexpected error occurred."
        });
    });
});

app.MapPost("/products", async 
    (CreateProductRequest request, 
    IProductService productService,
    CancellationToken cancellationToken) =>
{
    var validationError = ValidateProductRequest(request);
    if (validationError is not null)
    {
        return Results.BadRequest(new
        {
            error = validationError
        });
    }
    var response = await productService.Create(request, cancellationToken);
    return Results.Created($"/products/{response.Id}", response);
});

app.MapGet("/products", async(IProductService productService,
    CancellationToken cancellationToken)=>
{ 
    var response =  await productService.GetAll(cancellationToken);
    return Results.Ok(response);
}
);

app.MapGet("/products/{id:int}", async 
    (int id,
    IProductService productService,
    CancellationToken cancellationToken) =>
{
    var response = await productService.GetById(id, cancellationToken);
    if (response is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(response);
}
);

app.MapPut("/products/{id:int}", async
    (int id,
    IProductService productService,
    CreateProductRequest request,
    CancellationToken cancellationToken) =>
{
    var validationError = ValidateProductRequest(request);

    if (validationError is not null)
    {
        return Results.BadRequest(new
        {
            error = validationError
        });
    }
    var response = await productService.Update(id, request, cancellationToken);
    if (response is null)
        return Results.NotFound();
    return Results.Ok(response);
}
);

app.MapDelete("/products/{id:int}", async
    (int id,
    IProductService productService,
    CancellationToken cancallationToken)=>
{
    var response = await productService.Delete(id, cancallationToken);
    if (response)
        return Results.NoContent();
    return Results.NotFound();
}
);

app.Run();
