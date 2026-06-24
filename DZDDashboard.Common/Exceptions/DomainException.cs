namespace DZDDashboard.Common.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
}

public sealed class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(string entityName, object key)
        : base($"{entityName} with id '{key}' was not found.") { }

    public EntityNotFoundException(string message)
        : base(message) { }
}

public sealed class DomainValidationException : DomainException
{
    public DomainValidationException(string message) : base(message) { }
}

public sealed class DomainConflictException : DomainException
{
    public DomainConflictException(string message) : base(message) { }
}
