using Domain.Models.Locations;

namespace Application.Entities.LocationTypes.Exceptions
{
    public abstract class LocationTypeException(LocationTypeId id, string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
        public LocationTypeId LocationTypeId { get; } = id;
    }

    public class LocationTypeNotFoundException(LocationTypeId id)
        : LocationTypeException(id, $"Location type with id '{id}' was not found.");

    public class LocationTypeAlreadyExistsException(LocationTypeId id)
        : LocationTypeException(id, $"Location type with id '{id}' already exists.");

    public class UnhandledLocationTypeException(LocationTypeId id, Exception? innerException = null)
        : LocationTypeException(id, "Unexpected error occured", innerException);
}