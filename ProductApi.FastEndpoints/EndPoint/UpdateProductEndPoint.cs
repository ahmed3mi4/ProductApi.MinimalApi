using FastEndpoints;
using ProductApi.MinimalApi.DTOs;
using ProductApi.MinimalApi.Services;

namespace ProductApi.FastEndpoints.EndPoint;

public class UpdateProductEndPoint : Endpoint<UpdateProductRequest, ProductResponse> 
{
    private readonly IProductService _productService;
    public UpdateProductEndPoint(IProductService productService)
    {
        _productService = productService;
    }
    public override void Configure()
    {
        Put("/products/{id}");
        AllowAnonymous();
    }
    public override async Task HandleAsync(UpdateProductRequest req, CancellationToken ct)
    {
        var createRequest = new CreateProductRequest
        {
            Name = req.Name,
            Price = req.Price
        };
        var response = await _productService.Update(req.Id, createRequest, ct );
        if (response is null)
        {
            await Send.NotFoundAsync(cancellation: ct);
            return;
        }
        await Send.OkAsync(response, cancellation: ct);
    }

}
