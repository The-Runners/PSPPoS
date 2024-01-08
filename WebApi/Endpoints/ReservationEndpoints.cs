using Contracts.DTOs;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;

namespace WebApi.Endpoints;

public static class ReservationEndpoints
{
    public static void MapReservationEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/reservation").WithTags("Reservations");

        group.MapPost("/create", async (
            [FromServices] IReservationService reservationService,
            ReservationOrderDto reservationDto) =>
        {
            await reservationService.CreateReservation(reservationDto);
        });

        group.MapPut("{reservationId}/cancel", async (
            [FromServices] IReservationService reservationService,
            Guid reservationId) =>
        {
            await reservationService.CancelReservation(reservationId);
        });
    }
}