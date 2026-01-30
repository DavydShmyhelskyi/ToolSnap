using Application.Common.Interfaces.Repositories;
using Application.ToolAssignments.Exceptions;
using Domain.Models.Locations;
using Domain.Models.ToolAssignments;
using Domain.Models.Tools;
using Domain.Models.Users;
using LanguageExt;
using MediatR;

namespace Application.ToolAssignments.Commands
{
    public record CreateToolAssignmentCommand : IRequest<Either<ToolAssignmentException, ToolAssignment>>
    {
        public required Guid ToolId { get; init; }
        public required Guid UserId { get; init; }
        public required Guid LocationId { get; init; }
    }

    public class CreateToolAssignmentCommandHandler(
        IToolAssigmentsRepository repository)
        : IRequestHandler<CreateToolAssignmentCommand, Either<ToolAssignmentException, ToolAssignment>>
    {
        public async Task<Either<ToolAssignmentException, ToolAssignment>> Handle(
            CreateToolAssignmentCommand request,
            CancellationToken cancellationToken)
        {
            return await CreateEntity(request, cancellationToken);
        }

        private async Task<Either<ToolAssignmentException, ToolAssignment>> CreateEntity(
            CreateToolAssignmentCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var toolId = new ToolId(request.ToolId);
                var userId = new UserId(request.UserId);
                var locationId = new LocationId(request.LocationId);

                var newAssignment = ToolAssignment.New(
                    toolId,
                    userId,
                    locationId,
                    DateTimeOffset.UtcNow);

                var result = await repository.Approve(cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return new UnhandledToolAssignmentException(ToolAssignmentId.Empty(), ex);
            }
        }
    }
}