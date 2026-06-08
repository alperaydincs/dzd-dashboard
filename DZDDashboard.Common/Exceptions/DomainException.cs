namespace DZDDashboard.Common.Exceptions;

/// <summary>Base class for all domain-level exceptions that map to client errors (4xx).</summary>
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
}

/// <summary>Entity was not found. Maps to HTTP 404.</summary>
public sealed class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(string entityName, object key)
        : base($"{entityName} with id '{key}' was not found.") { }

    public EntityNotFoundException(string message)
        : base(message) { }
}

/// <summary>Business rule / input validation error. Maps to HTTP 400.</summary>
public sealed class DomainValidationException : DomainException
{
    public DomainValidationException(string message) : base(message) { }
}

/// <summary>Conflict — state prevents the operation. Maps to HTTP 409.</summary>
public sealed class DomainConflictException : DomainException
{
    public DomainConflictException(string message) : base(message) { }
}
