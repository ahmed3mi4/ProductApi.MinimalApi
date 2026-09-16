using Microsoft.EntityFrameworkCore;
using ProductApi.MinimalApi.Data;
using ProductApi.MinimalApi.DTOs;
using ProductApi.MinimalApi.Models;

namespace ProductApi.MinimalApi.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProductService> _logger;
    public ProductService(AppDbContext context, ILogger<ProductService> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<ProductResponse> Create(CreateProductRequest request, CancellationToken cancellationToken)
    {

        var product = new Product
        {
            Name = request.Name,
            Price = request.Price,
            CreatedAtUtc = DateTime.UtcNow
        };

        _context.Products.Add(product);

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
                "Product created successfully. ProductId: {ProductId}",
                product.Id);

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            CreatedAtUtc = product.CreatedAtUtc
        };

    }
    public async Task<IEnumerable<ProductResponse>> GetAll(CancellationToken cancellationToken)
    {

        var products = await _context.Products
            .AsNoTracking()
            .Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CreatedAtUtc = p.CreatedAtUtc
            }).ToListAsync(cancellationToken);

        _logger.LogInformation(
             "Retrieved {ProductCount} products",
                        products.Count);
        return products;

    }

    public async Task<ProductResponse?> GetById(int id, CancellationToken cancellationToken)
    {
        var product = await _context.Products
             .AsNoTracking()
             .Select(p => new ProductResponse
             {
                 Id = p.Id,
                 Name = p.Name,
                 Price = p.Price,
                 CreatedAtUtc = p.CreatedAtUtc
             }).FirstOrDefaultAsync((p => p.Id == id), cancellationToken);

        if (product is null)
        {
            _logger.LogWarning(
                "Product not found. ProductId: {ProductId}",
                id);

            return null;
        }

        _logger.LogInformation(
            "Product retrieved successfully. ProductId: {ProductId}",
            id);

        return product;

    }

    public async Task<ProductResponse?> Update(int id, CreateProductRequest request, CancellationToken cancellationToken)
    {

        var effectedRows = await _context.Products
         .Where(p => p.Id == id)
         .ExecuteUpdateAsync(p => p
             .SetProperty(p => p.Name, request.Name)
             .SetProperty(p => p.Price, request.Price), cancellationToken);
        if (effectedRows == 0)
        {
            _logger.LogWarning(
       "Product not found for update. ProductId: {ProductId}",
               id);
            return null;
        }
        _logger.LogInformation(
    "Product updated successfully. ProductId: {ProductId}",
            id);
        return await GetById(id, cancellationToken);


    }
    public async Task<bool> Delete(int id, CancellationToken cancellationToken)
    {

        var effectedRows = await _context.Products
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
        if (effectedRows == 0)
        {
            _logger.LogWarning(
                "Product not found for deletion. ProductId: {ProductId}",
                id);

            return false;
        }

        _logger.LogInformation(
            "Product deleted successfully. ProductId: {ProductId}",
            id);

        return true;

    }

}


