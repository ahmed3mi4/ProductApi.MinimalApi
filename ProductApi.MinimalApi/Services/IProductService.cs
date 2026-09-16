using ProductApi.MinimalApi.DTOs;
using ProductApi.MinimalApi.Models;

namespace ProductApi.MinimalApi.Services;

public interface IProductService
{
    Task<ProductResponse> Create(CreateProductRequest request , CancellationToken cancellationToken);
    Task<IEnumerable<ProductResponse>> GetAll(CancellationToken cancellationToken);
    Task<ProductResponse?> GetById(int id, CancellationToken cancellationToken);
    Task<ProductResponse?> Update(int id, CreateProductRequest request, CancellationToken cancellationToken);
    Task<bool> Delete(int id, CancellationToken cancellationToken);

}
