using Domain.Exceptions;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Extensions;

public static class EitherExtensions
{
    public static IResult ToHttpResult<T>(this Either<DomainException, T> either) => either.Match<IResult>(
            success => Results.Ok(success),
            error => error is NotFoundException
                ? Results.NotFound(error.Message)
                : Results.BadRequest(error.Message));

    public static async Task<IResult> ToHttpResult<T>(this Task<Either<DomainException, T>> task) =>
        (await task).ToHttpResult();
}
