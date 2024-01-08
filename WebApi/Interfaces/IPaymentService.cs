using Contracts.DTOs;
using Domain.Exceptions;
using Domain.Models;
using LanguageExt;

namespace WebApi.Interfaces;

public interface IPaymentService
{
    ValueTask<Either<DomainException, Payment>> GetPaymentAsync(Guid id);

    Task<Either<DomainException, Payment>> AddPaymentAsync(PaymentCreateDto paymentCreateDto);
}