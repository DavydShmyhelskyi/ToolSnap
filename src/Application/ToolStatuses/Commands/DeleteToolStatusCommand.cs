using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.ToolStatuses.Exceptions;
using Domain.Models.Tools;
using LanguageExt;
using MediatR;

namespace Application.ToolStatuses.Commands
{
    public record DeleteToolStatusCommand : IRequest<Either<ToolStatusException, ToolStatus>>
    {
        public required Guid ToolStatusId { get; init; }
    }

    public class DeleteToolStatusCommandHandler(
        IToolStatusQueries queries,
        IToolStatusRepository repository)
        : IRequestHandler<DeleteToolStatusCommand, Either<ToolStatusException, ToolStatus>>
    {
        public async Task<Either<ToolStatusException, ToolStatus>> Handle(
            DeleteToolStatusCommand request,
            CancellationToken cancellationToken)
        {
            var id = new ToolStatusId(request.ToolStatusId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return entity.Match<Either<ToolStatusException, ToolStatus>>(
                ts => repository.DeleteAsync(ts, cancellationToken).Result,
                () => new ToolStatusNotFoundException(id));
        }
    }
}