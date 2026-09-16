using FastEndpoints;
using ProductApi.MinimalApi.DTOs;
using ProductApi.MinimalApi.Services;

namespace ProductApi.FastEndpoints.EndPoint;

public class GetProductByIdEndPoint : Endpoint<GetProductByIdRequest, ProductResponse>
{
    private readonly IProductService _productService;
    public GetProductByIdEndPoint(IProductService productService)
    {
        _productService = productService;
    }
    public override void Configure()
    {
        Get("/products/{id}");
        AllowAnonymous();
    }
    public override async Task HandleAsync(GetProductByIdRequest req, CancellationToken ct)
    {
        var response = await _productService.GetById(req.Id, ct);
        if (response is null)
        {
            await Send.NotFoundAsync(cancellation: ct);
            return;
        }
        await Send.OkAsync(response, cancellation: ct);
    }
}
