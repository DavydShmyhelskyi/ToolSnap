using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.ToolStatuses.Exceptions;
using Domain.Models.ToolInfo;
using Domain.Models.Tools;
using LanguageExt;
using MediatR;

namespace Application.Entities.ToolStatuses.Commands
{
    public record UpdateToolStatusCommand : IRequest<Either<ToolStatusException, ToolStatus>>
    {
        public required Guid ToolStatusId { get; init; }
        public required string Title { get; init; }
    }

    public class UpdateToolStatusCommandHandler(
        IToolStatusQueries queries,
        IToolStatusRepository repository)
        : IRequestHandler<UpdateToolStatusCommand, Either<ToolStatusException, ToolStatus>>
    {
        public async Task<Either<ToolStatusException, ToolStatus>> Handle(
            UpdateToolStatusCommand request,
            CancellationToken cancellationToken)
        {
            var id = new ToolStatusId(request.ToolStatusId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return await entity.MatchAsync(
                ts => UpdateEntity(ts, request, cancellationToken),
                () => new ToolStatusNotFoundException(id));
        }

        private async Task<Either<ToolStatusException, ToolStatus>> UpdateEntity(
            ToolStatus entity,
            UpdateToolStatusCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                entity.Update(request.Title);
                return await repository.UpdateAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledToolStatusException(entity.Id, ex);
            }
        }
    }
}