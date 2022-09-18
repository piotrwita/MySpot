namespace MySpot.Core.Exceptions;

public sealed class InvalidEntityIdException : CustomException
{
    public Guid Id { get; }

    public InvalidEntityIdException(Guid id)
        : base($"Entity id: {id} is invalid.")
    {
        Id = id;
    }
}