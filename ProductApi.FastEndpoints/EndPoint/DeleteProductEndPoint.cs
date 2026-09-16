using FastEndpoints;
using ProductApi.MinimalApi.Services;

namespace ProductApi.FastEndpoints.EndPoint;

public class DeleteProductEndPoint : Endpoint<DeleteProductRequest>
{
    private readonly IProductService _productService;
    public DeleteProductEndPoint(IProductService productService)
    {
        _productService = productService;
    }
    public override void Configure()
    {
        Delete("/products/{id}");
        AllowAnonymous();
    }
    public override async Task HandleAsync(DeleteProductRequest req, CancellationToken ct)
    {
        var response = await _productService.Delete(req.Id, ct);
        if (response)
            await Send.NoContentAsync(ct);
        else
            await Send.NotFoundAsync(ct);
    }
}

