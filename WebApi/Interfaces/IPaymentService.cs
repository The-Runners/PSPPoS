using Contracts.DTOs.Payment;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Interfaces;
using LanguageExt;

namespace WebApi.Interfaces;

public interface IPaymentService
{
    ValueTask<Either<DomainException, Payment>> GetPaymentAsync(Guid id);
    ValueTask<Either<DomainException, Payment>> AddPaymentAsync(PaymentCreateDto paymentCreateDto);
}