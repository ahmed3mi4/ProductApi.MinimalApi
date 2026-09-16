namespace ProductApi.MinimalApi.DTOs;

public class CreateProductRequest
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
}
