using System.Security.Claims;
using ECommerce.Api.DTOs;
using ECommerce.Api.Models;
using ECommerce.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var userId = GetCurrentUserId();
        var cart = await _cartService.GetCartAsync(userId);

        return Ok(cart);
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddToCart(AddToCartDto dto)
    {
        var userId = GetCurrentUserId();

        var cart = await _cartService.AddToCartAsync(userId, dto);

        return Ok(cart);
    }

    [HttpPut("items/{productId}")]
    public async Task<IActionResult> UpdateCartItem(int productId, UpdateCartItemDto dto)
    {
        var userId = GetCurrentUserId();

        var cart = await _cartService.UpdateCartItemAsync(userId, productId, dto);  

        if (cart == null)
        {
            return NotFound();
        }

        return Ok(cart);
    }

    [HttpDelete("items/{productId}")]
    public async Task<IActionResult> RemoveFromCart(int productId)
    {
        var userId = GetCurrentUserId();
        var removed = await _cartService.RemoveFromCartAsync(userId, productId);

        if(!removed)
        {
            return NotFound();
        }

        return NoContent(); 
    }

    // private methods

    private int GetCurrentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        return int.Parse(userId!);
    }
}