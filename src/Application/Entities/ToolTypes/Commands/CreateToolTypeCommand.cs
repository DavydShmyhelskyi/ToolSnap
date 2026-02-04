using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.ToolTypes.Exceptions;
using Domain.Models.ToolInfo;
using LanguageExt;
using MediatR;

namespace Application.Entities.ToolTypes.Commands
{
    public record CreateToolTypeCommand : IRequest<Either<ToolTypeException, ToolType>>
    {
        public required string Title { get; init; }
    }

    public class CreateToolTypeCommandHandler(
        IToolTypeQueries queries,
        IToolTypeRepository repository)
        : IRequestHandler<CreateToolTypeCommand, Either<ToolTypeException, ToolType>>
    {
        public async Task<Either<ToolTypeException, ToolType>> Handle(
            CreateToolTypeCommand request,
            CancellationToken cancellationToken)
        {
            var existing = await queries.GetByTitleAsync(request.Title, cancellationToken);

            return await existing.MatchAsync(
                t => new ToolTypeAlreadyExistsException(t.Id),
                () => CreateEntity(request, cancellationToken));
        }

        private async Task<Either<ToolTypeException, ToolType>> CreateEntity(
            CreateToolTypeCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var newToolType = ToolType.New(request.Title);
                var result = await repository.AddAsync(newToolType, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return new UnhandledToolTypeException(ToolTypeId.Empty(), ex);
            }
        }
    }
}