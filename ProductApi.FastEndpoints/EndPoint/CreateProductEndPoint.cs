using FastEndpoints;
using ProductApi.FastEndpoints.EndPoint;
using ProductApi.MinimalApi.DTOs;
using ProductApi.MinimalApi.Services;

namespace ProductApi.FastEndpoints.EndPoints;

public class CreateProductEndPoint : Endpoint<CreateProductRequest, ProductResponse>
{
    private readonly IProductService _productService;

    public CreateProductEndPoint(IProductService productService)
    {
        _productService = productService;
    }
    public override void Configure()
    {
        Post("/products");
        AllowAnonymous();

    }
    public override async Task HandleAsync(CreateProductRequest req, CancellationToken ct)
    {
        var response = await _productService.Create(req, ct);
        await Send.CreatedAtAsync<GetProductByIdEndPoint>(
            new {id = response.Id },
            response,
            cancellation: ct
            );
    }
}
