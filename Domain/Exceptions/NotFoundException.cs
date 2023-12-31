namespace Domain.Exceptions;

public class NotFoundException(string domain, object id)
: DomainException($"{domain} with id `{id}` was not found.")
{

}
