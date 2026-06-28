using AutoMapper;
using ECommerce.Api.Data;
using ECommerce.Api.DTOs;
using ECommerce.Api.Models;
using ECommerce.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Services;

public class CartService : ICartService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CartService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CartDto> AddToCartAsync(int userId, AddToCartDto dto)
    {
        var productExists = await _context.Products.AnyAsync(p => p.Id == dto.ProductId);

        if (!productExists)
            throw new KeyNotFoundException("Product not found.");

        var cart = await GetOrCreateCartAsync(userId);

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);

        if (existingItem != null)
            existingItem.Quantity += dto.Quantity;
        else
            cart.Items.Add(new CartItem
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            });
        
        await _context.SaveChangesAsync();
        return _mapper.Map<CartDto>(cart);
    }

    public async Task<CartDto> GetCartAsync(int userId)
    {
        var cart = await GetOrCreateCartAsync(userId);

        return _mapper.Map<CartDto>(cart);
    }

    public async Task<bool> RemoveFromCartAsync(int userId, int productId)
    {
        var cart = await GetOrCreateCartAsync(userId);

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if ( item == null )
        {
            return false;
        }

        _context.CartItems.Remove(item);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<CartDto?> UpdateCartItemAsync(int userId, int productId, UpdateCartItemDto dto)
    {
        var cart = await GetOrCreateCartAsync(userId);

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (item == null)
            return null;

        item.Quantity = dto.Quantity;
        await _context.SaveChangesAsync();

        return _mapper.Map<CartDto>(cart);
    }

    // private methods:

    private async Task<Cart> GetOrCreateCartAsync(int userId)
    {
        var cart = await _context.Carts.Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart != null)
        {
            return cart;
        }

        cart = new Cart
        {
            UserId = userId,
        };

        _context.Carts.Add(cart);
        await _context.SaveChangesAsync();

        return await _context.Carts.Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }
}