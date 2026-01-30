using Domain.Models.Locations;

namespace Application.Locations.Exceptions
{
    public abstract class LocationException(LocationId id, string message, Exception? innerException = null)
        :Exception(message, innerException)
    {
        public LocationId LocationId { get; } = id;

    }

    public class LocationNotFoundException(LocationId id)
        : LocationException(id, $"Location with id '{id}' was not found.");

    public class LocationAlreadyExistsException(LocationId id)
        : LocationException(id, $"Location with id '{id}' already exists.");

    public class UnhandledLocationException(LocationId id, Exception? innerException = null)
        : LocationException(id, "Unexpected error occured", innerException);
}
