using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.ActionTypes.Exceptions;
using Domain.Models.PhotoSessions;
using LanguageExt;
using MediatR;

namespace Application.Entities.ActionTypes.Commands
{
    public record UpdateActionTypeCommand : IRequest<Either<ActionTypeException, ActionType>>
    {
        public required Guid ActionTypeId { get; init; }
        public required string Title { get; init; }
    }

    public class UpdateActionTypeCommandHandler(
        IActionTypeQueries queries,
        IActionTypeRepository repository)
        : IRequestHandler<UpdateActionTypeCommand, Either<ActionTypeException, ActionType>>
    {
        public async Task<Either<ActionTypeException, ActionType>> Handle(
            UpdateActionTypeCommand request,
            CancellationToken cancellationToken)
        {
            var id = new ActionTypeId(request.ActionTypeId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return await entity.MatchAsync(
                at => UpdateEntity(at, request, cancellationToken),
                () => new ActionTypeNotFoundException(id));
        }

        private async Task<Either<ActionTypeException, ActionType>> UpdateEntity(
            ActionType entity,
            UpdateActionTypeCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                entity.ChangeTitle(request.Title);
                return await repository.UpdateAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledActionTypeException(entity.Id, ex);
            }
        }
    }
}