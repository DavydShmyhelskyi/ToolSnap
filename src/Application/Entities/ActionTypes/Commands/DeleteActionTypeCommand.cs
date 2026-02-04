using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.ActionTypes.Exceptions;
using Domain.Models.PhotoSessions;
using LanguageExt;
using MediatR;

namespace Application.Entities.ActionTypes.Commands
{
    public record DeleteActionTypeCommand : IRequest<Either<ActionTypeException, ActionType>>
    {
        public required Guid ActionTypeId { get; init; }
    }

    public class DeleteActionTypeCommandHandler(
        IActionTypeQueries queries,
        IActionTypeRepository repository)
        : IRequestHandler<DeleteActionTypeCommand, Either<ActionTypeException, ActionType>>
    {
        public async Task<Either<ActionTypeException, ActionType>> Handle(
            DeleteActionTypeCommand request,
            CancellationToken cancellationToken)
        {
            var id = new ActionTypeId(request.ActionTypeId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return entity.Match<Either<ActionTypeException, ActionType>>(
                at => repository.DeleteAsync(at, cancellationToken).Result,
                () => new ActionTypeNotFoundException(id));
        }
    }
}