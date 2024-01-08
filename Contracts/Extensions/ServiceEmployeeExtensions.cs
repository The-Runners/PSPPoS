using Contracts.DTOs;
using Domain.Models;

namespace Contracts.Extensions;

public static class ServiceEmployeeExtensions
{
    public static ServiceEmployeeModelDto ToModelDto(this ServiceEmployee serviceEmployee) => new()
    {
        EmployeeId = serviceEmployee.EmployeeId,
        ServiceId = serviceEmployee.ServiceId,
    };
}