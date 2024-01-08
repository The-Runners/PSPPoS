using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;

namespace WebApi.Endpoints;

public static class EmployeeEndpoints
{
    public static void MapEmployeeEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/employee").WithTags("Employees");

        group.MapGet("{id}/show-available-times", async(
            [FromServices] IEmployeeService employeeService,
            [FromBody] TimeSlot timePeriod,
            Guid id) =>
        {
            var result = await employeeService.GetAvailableTimeSlots(id, timePeriod);
            return result;
        });
    }
}