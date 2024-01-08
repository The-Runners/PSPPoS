namespace Contracts.DTOs;

public class OrderProductsDto
{
    public required Guid OrderId { get; set; }

    public required IEnumerable<OrderProductSingleDto> OrderProducts { get; set; }
}