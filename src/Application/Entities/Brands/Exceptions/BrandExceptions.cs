using Domain.Models.ToolInfo;

namespace Application.Entities.Brands.Exceptions
{
    public abstract class BrandException(BrandId id, string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
        public BrandId BrandId { get; } = id;
    }

    public class BrandNotFoundException(BrandId id)
        : BrandException(id, $"Brand with id '{id}' was not found.");

    public class BrandAlreadyExistsException(BrandId id)
        : BrandException(id, $"Brand with id '{id}' already exists.");

    public class UnhandledBrandException(BrandId id, Exception? innerException = null)
        : BrandException(id, "Unexpected error occured", innerException);
}