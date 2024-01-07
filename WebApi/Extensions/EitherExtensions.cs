using Domain.Exceptions;
using LanguageExt;

namespace WebApi.Extensions;

public static class EitherExtensions
{
    public static IResult ToHttpResult<T>(this Either<DomainException, T> either) => either.Match<IResult>(
            success => Results.Ok(success),
            error => error is NotFoundException
                ? Results.NotFound(error.Message)
                : Results.BadRequest(error.Message));
}
