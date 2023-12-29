using Contracts.DTOs.Reservation;
using Domain.Models;
using Infrastructure.Interfaces;
using WebApi.Interfaces;

namespace WebApi.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IGenericRepository<Service> _serviceRepository;

    public ReservationService(
        IReservationRepository reservationRepository,
        IGenericRepository<Service> serviceRepository)
    {
        _reservationRepository = reservationRepository;
        _serviceRepository = serviceRepository;
    }
    public async Task CreateReservation(ReservationOrderDto reservationDto)
    {
        // TO-DO
    }

    public async Task<ReservationServiceDto> GenerateReservationServiceModel(Guid orderId)
    {
        var reservationServiceDto = new ReservationServiceDto(); 
        var reservation = await _reservationRepository.GetReservationByOrderId(orderId);
        if (reservation is null)
        {
            return reservationServiceDto;
        }

        reservationServiceDto.ReservationId = reservation.Id;
        reservationServiceDto.TimeSlot = reservation.TimeSlot;
        var service = await _serviceRepository.GetById(reservation.ServiceId);
        if (service is null)
        {
            return reservationServiceDto;
        }

        reservationServiceDto.ServiceId = service.Id;
        reservationServiceDto.Name = service.Name;
        reservationServiceDto.Duration = service.Duration;

        return reservationServiceDto;
    }
}