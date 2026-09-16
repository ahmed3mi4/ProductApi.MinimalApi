namespace ProductApi.FastEndpoints.EndPoint;

public class UpdateProductRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
}
