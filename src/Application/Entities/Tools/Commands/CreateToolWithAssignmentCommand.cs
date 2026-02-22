using System.IO;
using Application.Common.Interfaces.Services;
using Domain.Models.Tools;
using LanguageExt;
using MediatR;

namespace Application.Entities.Tools.Commands
{
    /// <summary>
    /// Команда, яка створює Tool і одразу ж піднімає повний flow:
    /// ToolPhoto, PhotoSession, PhotoForDetection, DetectedTool, ToolAssignment (take + return).
    /// На виході повертає тільки Tool.
    /// </summary>
    public record CreateToolWithAssignmentCommand
        : IRequest<Either<Exception, Tool>>
    {
        public required Guid UserId { get; init; }
        public required Guid ActionTypeId { get; init; }
        public required Guid PhotoTypeId { get; init; }
        public required Guid LocationId { get; init; }

        public required Guid ToolTypeId { get; init; }
        public required Guid ToolStatusId { get; init; }
        public Guid? BrandId { get; init; }
        public Guid? ModelId { get; init; }
        public string? SerialNumber { get; init; }

        public required string OriginalName { get; init; }
        public required Stream FileStream { get; init; }
    }

    public class CreateToolWithAssignmentCommandHandler
        : IRequestHandler<CreateToolWithAssignmentCommand, Either<Exception, Tool>>
    {
        private readonly IToolWithAssignmentService _service;

        public CreateToolWithAssignmentCommandHandler(
            IToolWithAssignmentService service)
        {
            _service = service;
        }

        public async Task<Either<Exception, Tool>> Handle(
            CreateToolWithAssignmentCommand request,
            CancellationToken cancellationToken)
        {
            return await _service.CreateToolWithAssignmentAsync(
                userId: request.UserId,
                actionTypeId: request.ActionTypeId,
                photoTypeId: request.PhotoTypeId,
                locationId: request.LocationId,
                toolTypeId: request.ToolTypeId,
                toolStatusId: request.ToolStatusId,
                brandId: request.BrandId,
                modelId: request.ModelId,
                serialNumber: request.SerialNumber,
                originalName: request.OriginalName,
                fileStream: request.FileStream,
                cancellationToken: cancellationToken);
        }
    }
}