using Domain.Models.Locations;

namespace Application.Entities.Locations.Exceptions
{
    public abstract class LocationException(LocationId id, string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
        public LocationId LocationId { get; } = id;
    }

    public class LocationNotFoundException(LocationId id)
        : LocationException(id, $"Location with id '{id}' was not found.");

    public class LocationAlreadyExistsException(LocationId id)
        : LocationException(id, $"Location with id '{id}' already exists.");

    public class LocationTypeNotFoundForLocationException(LocationTypeId locationTypeId)
        : LocationException(LocationId.Empty(), $"Location type with id '{locationTypeId}' was not found. Cannot create location.")
    {
        public LocationTypeId LocationTypeId { get; } = locationTypeId;
    }

    public class UnhandledLocationException(LocationId id, Exception? innerException = null)
        : LocationException(id, "Unexpected error occured", innerException);
}
