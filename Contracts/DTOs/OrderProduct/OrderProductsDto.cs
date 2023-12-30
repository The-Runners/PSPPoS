namespace Contracts.DTOs.OrderProduct;

public class OrderProductsDto
{
    public required Guid OrderId { get; set; }

    public required IEnumerable<OrderProductSingleDto> OrderProducts { get; set; }
}