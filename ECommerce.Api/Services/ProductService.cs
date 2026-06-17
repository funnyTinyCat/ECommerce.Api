using AutoMapper;
using ECommerce.Api.Data;
using ECommerce.Api.DTOs;
using ECommerce.Api.Models;
using ECommerce.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
    public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
    {
        var product = _mapper.Map<Product>(dto);

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<(IEnumerable<ProductDto> Data, int TotalCount)> GetAllProductsAsync(
        ProductQueryParameters query)
    {
        var productsQuery = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            productsQuery = productsQuery.Where( p => p.Name.Contains(query.Search));
        }

        if (query.MinPrice.HasValue)
        {
            productsQuery = productsQuery.Where( p => p.Price >= query.MinPrice.Value);
        }

        if (query.MaxPrice.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.Price <= query.MaxPrice.Value);
        }
        
        var totalCount = await productsQuery.CountAsync();

        productsQuery = query.SortBy?.ToLower() switch
        {
            "price" => query.Descending 
                ? productsQuery.OrderByDescending(p => p.Price) 
                : productsQuery.OrderBy(p => p.Price),

            "name" => query.Descending
                ? productsQuery.OrderByDescending(p => p.Name)
                : productsQuery.OrderBy(p => p.Name),
            
            _ => productsQuery.OrderBy(p => p.Id)
        };

        var products = await productsQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        var data = _mapper.Map<IEnumerable<ProductDto>>(products);

        return (data, totalCount);
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return null;

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto?> UpdateProductAsync(int id, CreateProductDto dto)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return null;

        _mapper.Map(dto, product);
        await _context.SaveChangesAsync();

        return _mapper.Map<ProductDto>(product);

    }
}