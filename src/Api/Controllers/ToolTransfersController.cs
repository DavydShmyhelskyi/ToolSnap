using Api.DTOs;
using Api.Modules.Errors;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using Application.Entities.ToolTransfers.Commands;
using Domain.Models.Tools;
using Domain.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("tool-transfers")]
    public class ToolTransfersController(
        IToolTransferQueries queries,
        IToolTransferControllerService service,
        ISender sender) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(IReadOnlyList<ToolTransferDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ToolTransferDto>>> GetAll(
            CancellationToken cancellationToken)
        {
            var transfers = await queries.GetAllAsync(cancellationToken);
            return Ok(transfers.Select(ToolTransferDto.FromDomain).ToList());
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(ToolTransferDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToolTransferDto>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var result = await service.Get(id, cancellationToken);

            return result.Match<ActionResult<ToolTransferDto>>(
                dto => Ok(dto),
                () => NotFound());
        }

        [HttpGet("tool/{toolId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(IReadOnlyList<ToolTransferDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ToolTransferDto>>> GetByTool(
            Guid toolId,
            CancellationToken cancellationToken)
        {
            var transfers = await queries.GetAllByToolIdAsync(new ToolId(toolId), cancellationToken);
            return Ok(transfers.Select(ToolTransferDto.FromDomain).ToList());
        }

        [HttpGet("initiated-by/{userId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(IReadOnlyList<ToolTransferDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ToolTransferDto>>> GetInitiatedByUser(
            Guid userId,
            CancellationToken cancellationToken)
        {
            var transfers = await queries.GetAllByFromUserIdAsync(new UserId(userId), cancellationToken);
            return Ok(transfers.Select(ToolTransferDto.FromDomain).ToList());
        }

        [HttpGet("pending/to/{userId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(IReadOnlyList<ToolTransferDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ToolTransferDto>>> GetPendingForUser(
            Guid userId,
            CancellationToken cancellationToken)
        {
            var transfers = await queries.GetPendingByToUserIdAsync(new UserId(userId), cancellationToken);
            return Ok(transfers.Select(ToolTransferDto.FromDomain).ToList());
        }

        [HttpGet("received-by/{userId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(IReadOnlyList<ToolTransferDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ToolTransferDto>>> GetReceivedByUser(
            Guid userId,
            CancellationToken cancellationToken)
        {
            var transfers = await queries.GetAllByToUserIdAsync(new UserId(userId), cancellationToken);
            return Ok(transfers.Select(ToolTransferDto.FromDomain).ToList());
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ToolTransferDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ToolTransferDto>> Initiate(
            [FromBody] InitiateToolTransferDto request,
            CancellationToken cancellationToken)
        {
            var command = new InitiateToolTransferCommand
            {
                FromUserId = request.FromUserId,
                ToUserId = request.ToUserId,
                ToolId = request.ToolId
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<ToolTransferDto>>(
                transfer => CreatedAtAction(
                    nameof(GetById),
                    new { id = transfer.Id.Value },
                    ToolTransferDto.FromDomain(transfer)),
                error => error.ToObjectResult());
        }

        [HttpPatch("{id:guid}/accept")]
        [Authorize]
        [ProducesResponseType(typeof(ToolTransferDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ToolTransferDto>> Accept(
            Guid id,
            [FromBody] RespondToToolTransferDto request,
            CancellationToken cancellationToken)
        {
            var command = new AcceptToolTransferCommand
            {
                TransferId = id,
                ToUserId = request.ResponderUserId
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<ToolTransferDto>>(
                transfer => Ok(ToolTransferDto.FromDomain(transfer)),
                error => error.ToObjectResult());
        }

        [HttpPatch("{id:guid}/reject")]
        [Authorize]
        [ProducesResponseType(typeof(ToolTransferDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ToolTransferDto>> Reject(
            Guid id,
            [FromBody] RespondToToolTransferDto request,
            CancellationToken cancellationToken)
        {
            var command = new RejectToolTransferCommand
            {
                TransferId = id,
                ToUserId = request.ResponderUserId
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<ToolTransferDto>>(
                transfer => Ok(ToolTransferDto.FromDomain(transfer)),
                error => error.ToObjectResult());
        }

        [HttpPatch("{id:guid}/cancel")]
        [Authorize]
        [ProducesResponseType(typeof(ToolTransferDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ToolTransferDto>> Cancel(
            Guid id,
            [FromBody] CancelToolTransferDto request,
            CancellationToken cancellationToken)
        {
            var command = new CancelToolTransferCommand
            {
                TransferId = id,
                FromUserId = request.InitiatorUserId
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<ToolTransferDto>>(
                transfer => Ok(ToolTransferDto.FromDomain(transfer)),
                error => error.ToObjectResult());
        }
    }
}
