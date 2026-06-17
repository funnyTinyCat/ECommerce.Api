using ECommerce.Api.DTOs;

namespace ECommerce.Api.Services.Interfaces;

public interface IProductService
{
    Task<(IEnumerable<ProductDto> Data, int TotalCount)> GetAllProductsAsync(ProductQueryParameters query);

    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<ProductDto> CreateProductAsync(CreateProductDto dto);
    Task<ProductDto?> UpdateProductAsync(int id, CreateProductDto dto);
    Task<bool> DeleteProductAsync(int id);
}