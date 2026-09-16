using FastEndpoints;
using ProductApi.MinimalApi.Data;
using ProductApi.MinimalApi.DTOs;
using ProductApi.MinimalApi.Services;

namespace ProductApi.FastEndpoints.EndPoint;

public class GetAllProductsEndPoint : EndpointWithoutRequest<IEnumerable<ProductResponse>>
{
    private readonly IProductService _productService;

    public GetAllProductsEndPoint(IProductService productService)
    {
        _productService = productService;
    }

    public override void Configure()
    {
        Get("/products");
        AllowAnonymous();
    }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await _productService.GetAll(ct);
        await Send.OkAsync(response.ToList(), cancellation: ct);
    }
}
