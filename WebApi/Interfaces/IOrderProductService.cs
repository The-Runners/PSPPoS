using Contracts.DTOs;

namespace WebApi.Interfaces;

public interface IOrderProductService
{
    Task<List<ProductForOrderDto>?> GenerateProductViewModels(Guid orderId);
}