using Domain.Models.ToolInfo;

namespace Application.Entities.Models.Exceptions
{
    public abstract class ModelException(ModelId id, string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
        public ModelId ModelId { get; } = id;
    }

    public class ModelNotFoundException(ModelId id)
        : ModelException(id, $"Model with id '{id}' was not found.");

    public class ModelAlreadyExistsException(ModelId id)
        : ModelException(id, $"Model with id '{id}' already exists.");

    public class UnhandledModelException(ModelId id, Exception? innerException = null)
        : ModelException(id, "Unexpected error occured", innerException);
}