using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.ActionTypes.Exceptions;
using Domain.Models.PhotoSessions;
using LanguageExt;
using MediatR;

namespace Application.Entities.ActionTypes.Commands
{
    public record CreateActionTypeCommand : IRequest<Either<ActionTypeException, ActionType>>
    {
        public required string Title { get; init; }
    }

    public class CreateActionTypeCommandHandler(
        IActionTypeQueries queries,
        IActionTypeRepository repository)
        : IRequestHandler<CreateActionTypeCommand, Either<ActionTypeException, ActionType>>
    {
        public async Task<Either<ActionTypeException, ActionType>> Handle(
            CreateActionTypeCommand request,
            CancellationToken cancellationToken)
        {
            var existing = await queries.GetByTitleAsync(request.Title, cancellationToken);

            return await existing.MatchAsync(
                at => new ActionTypeAlreadyExistsException(at.Id),
                () => CreateEntity(request, cancellationToken));
        }

        private async Task<Either<ActionTypeException, ActionType>> CreateEntity(
            CreateActionTypeCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var newActionType = ActionType.New(request.Title);
                var result = await repository.AddAsync(newActionType, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return new UnhandledActionTypeException(ActionTypeId.Empty(), ex);
            }
        }
    }
}