using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.DetectedTools.Exceptions;
using Domain.Models.DetectedTools;
using LanguageExt;
using MediatR;

namespace Application.Entities.DetectedTools.Commands
{
    public record DeleteDetectedToolCommand : IRequest<Either<DetectedToolException, DetectedTool>>
    {
        public required Guid DetectedToolId { get; init; }
    }

    public class DeleteDetectedToolCommandHandler(
        IDetectedToolQueries queries,
        IDetectedToolRepository repository)
        : IRequestHandler<DeleteDetectedToolCommand, Either<DetectedToolException, DetectedTool>>
    {
        public async Task<Either<DetectedToolException, DetectedTool>> Handle(
            DeleteDetectedToolCommand request,
            CancellationToken cancellationToken)
        {
            var id = new DetectedToolId(request.DetectedToolId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return entity.Match<Either<DetectedToolException, DetectedTool>>(
                dt => repository.DeleteAsync(dt, cancellationToken).Result,
                () => new DetectedToolNotFoundException(id));
        }
    }
}

// треба додати перевірку, що якщо це останній детектед тул в фотосесії, то треба видалити і фотосесію