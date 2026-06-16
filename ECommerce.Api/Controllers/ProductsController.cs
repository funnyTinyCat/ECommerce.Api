using AutoMapper;
using ECommerce.Api.Data;
using ECommerce.Api.DTOs;
using ECommerce.Api.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{

    private readonly AppDbContext _context;
    private readonly IValidator<CreateProductDto> _validator;
    private readonly IMapper _mapper;


    public ProductsController(AppDbContext context, IValidator<CreateProductDto> validator, IMapper mapper)
    {
        _context = context;
        _validator = validator;
        _mapper = mapper; 
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _context.Products.ToListAsync();
        var result = _mapper.Map<IEnumerable<ProductDto>>(products);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound();

        var result = _mapper.Map<ProductDto>(product);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductDto dto)
    {   

        var validationResult = await _validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        if (dto == null)
            return BadRequest();

        var product = _mapper.Map<Product>(dto);
        
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var result = _mapper.Map<ProductDto>(product);

        return CreatedAtAction(nameof(GetProductById), new {id = result.Id}, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, CreateProductDto dto)
    {
        if (dto == null)
            return BadRequest();

        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        _mapper.Map(product, dto);

        await _context.SaveChangesAsync();

        var result = _mapper.Map<ProductDto>(product);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound();

         _context.Products.Remove(product);
         await _context.SaveChangesAsync();

        return NoContent();
    }
}