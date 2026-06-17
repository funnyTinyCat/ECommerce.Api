using AutoMapper;
using ECommerce.Api.Data;
using ECommerce.Api.DTOs;
using ECommerce.Api.Models;
using ECommerce.Api.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{

    private readonly IProductService _productService;
    private readonly IValidator<CreateProductDto> _validator;

    public ProductsController(IProductService productService, IValidator<CreateProductDto> validator)
    {
        _productService = productService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        
        return Ok(products);
    }

    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById([FromRoute] int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductDto dto)
    {   

        var validationResult = await _validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        { 
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        var result = await _productService.CreateProductAsync(dto);

        return CreatedAtAction(nameof(GetProductById), new {id = result.Id}, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, CreateProductDto dto)
    {
        var product = await _productService.UpdateProductAsync(id, dto);

        if(product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _productService.DeleteProductAsync(id);

        if (product == false)
            return NotFound();

        return NoContent();
    }

    // [HttpGet("test-error")]
    // public IActionResult TestError()
    // {
    //     throw new Exception("This is a test exception from products controller."); 
    // }
}