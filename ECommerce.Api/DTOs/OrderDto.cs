using ECommerce.Api.Enums;

namespace ECommerce.Api.DTOs;


public class OrderDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }

    public List<OrderItemDto> Items { get; set; } = new (); 

}