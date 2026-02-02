using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.ToolStatuses.Exceptions;
using Domain.Models.Tools;
using LanguageExt;
using MediatR;

namespace Application.ToolStatuses.Commands
{
    public record CreateToolStatusCommand : IRequest<Either<ToolStatusException, ToolStatus>>
    {
        public required string Title { get; init; }
    }

    public class CreateToolStatusCommandHandler(
        IToolStatusQueries queries,
        IToolStatusRepository repository)
        : IRequestHandler<CreateToolStatusCommand, Either<ToolStatusException, ToolStatus>>
    {
        public async Task<Either<ToolStatusException, ToolStatus>> Handle(
            CreateToolStatusCommand request,
            CancellationToken cancellationToken)
        {
            var existing = await queries.GetByTitleAsync(request.Title, cancellationToken);

            return await existing.MatchAsync(
                ts => new ToolStatusAlreadyExistsException(ts.Id),
                () => CreateEntity(request, cancellationToken));
        }

        private async Task<Either<ToolStatusException, ToolStatus>> CreateEntity(
            CreateToolStatusCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var newToolStatus = ToolStatus.New(request.Title);
                var result = await repository.AddAsync(newToolStatus, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return new UnhandledToolStatusException(ToolStatusId.Empty(), ex);
            }
        }
    }
}