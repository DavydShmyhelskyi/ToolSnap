using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.ToolTypes.Exceptions;
using Domain.Models.ToolInfo;
using LanguageExt;
using MediatR;

namespace Application.Entities.ToolTypes.Commands
{
    public record UpdateToolTypeCommand : IRequest<Either<ToolTypeException, ToolType>>
    {
        public required Guid ToolTypeId { get; init; }
        public required string Title { get; init; }
    }

    public class UpdateToolTypeCommandHandler(
        IToolTypeQueries queries,
        IToolTypeRepository repository)
        : IRequestHandler<UpdateToolTypeCommand, Either<ToolTypeException, ToolType>>
    {
        public async Task<Either<ToolTypeException, ToolType>> Handle(
            UpdateToolTypeCommand request,
            CancellationToken cancellationToken)
        {
            var id = new ToolTypeId(request.ToolTypeId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return await entity.MatchAsync(
                t => UpdateEntity(t, request, cancellationToken),
                () => new ToolTypeNotFoundException(id));
        }

        private async Task<Either<ToolTypeException, ToolType>> UpdateEntity(
            ToolType entity,
            UpdateToolTypeCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                entity.Update(request.Title);
                return await repository.UpdateAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledToolTypeException(entity.Id, ex);
            }
        }
    }
}