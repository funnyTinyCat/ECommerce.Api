using AutoMapper;
using ECommerce.Api.Data;
using ECommerce.Api.DTOs;
using ECommerce.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ProductService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public Task<ProductDto> CreateProductAsync(CreateProductDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteProductAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _context.Products.ToListAsync();

        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return null;

        return _mapper.Map<ProductDto>(product);
    }

    public Task<ProductDto?> UpdateProductAsync(int id, CreateProductDto dto)
    {
        throw new NotImplementedException();
    }
}