using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.ToolTypes.Exceptions;
using Domain.Models.ToolInfo;
using LanguageExt;
using MediatR;

namespace Application.Entities.ToolTypes.Commands
{
    public record DeleteToolTypeCommand : IRequest<Either<ToolTypeException, ToolType>>
    {
        public required Guid ToolTypeId { get; init; }
    }

    public class DeleteToolTypeCommandHandler(
        IToolTypeQueries queries,
        IToolTypeRepository repository)
        : IRequestHandler<DeleteToolTypeCommand, Either<ToolTypeException, ToolType>>
    {
        public async Task<Either<ToolTypeException, ToolType>> Handle(
            DeleteToolTypeCommand request,
            CancellationToken cancellationToken)
        {
            var id = new ToolTypeId(request.ToolTypeId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return entity.Match<Either<ToolTypeException, ToolType>>(
                t => repository.DeleteAsync(t, cancellationToken).Result,
                () => new ToolTypeNotFoundException(id));
        }
    }
}