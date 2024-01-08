using Contracts.DTOs;
using Contracts.Extensions;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;
using WebApi.Interfaces;

namespace WebApi.Endpoints;

public static class ReservationEndpoints
{
    public static void MapReservationEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/reservation").WithTags("Reservations");

        group.MapGet(string.Empty, ListReservations);

        group.MapGet("{id}", (
            [FromServices] IReservationService service,
            Guid id) => service
            .GetById(id)
            .MapAsync(x => x.ToModelDto())
            .ToHttpResult());

        group.MapPut("{id}/cancel", async (
            [FromServices] IReservationService reservationService,
            Guid id) => await reservationService
            .CancelReservation(id)
            .MapAsync(x => x.ToModelDto())
            .ToHttpResult());

        group.MapPost("/create", async (
            [FromServices] IReservationService reservationService,
            ReservationOrderDto reservationDto) =>
        {
            await reservationService.CreateReservation(reservationDto);
        });
    }

    private static async Task<IResult> ListReservations(
        [FromServices] IReservationService reservationService,
        int offset = 0,
        int limit = 100)
    {
        var reservations = await reservationService.ListAsync(offset, limit);
        return Results.Ok(reservations.Select(x => x.ToModelDto()));
    }
}