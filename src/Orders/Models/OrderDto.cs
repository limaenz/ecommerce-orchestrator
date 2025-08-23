namespace ECommerce.Orders.Api.Models;

public record OrderDto(int Id, string ProductName, decimal Price, int Quantity);

