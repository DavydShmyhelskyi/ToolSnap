using Api.DTOs;
using Api.Modules.Errors;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using Application.Entities.ToolAssignments.Commands;
using Domain.Models.Tools;
using Domain.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("tool-assignments")]
    public class ToolAssignmentsController(
        IToolAssignmentQueries queries,
        IToolAssignmentControllerService service,
        ISender sender) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<ToolAssignmentDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ToolAssignmentDto>>> GetToolAssignments(
            CancellationToken cancellationToken)
        {
            var toolAssignments = await queries.GetAllAsync(cancellationToken);

            var result = toolAssignments
                .Select(ToolAssignmentDto.FromDomain)
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ToolAssignmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToolAssignmentDto>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var entity = await service.Get(id, cancellationToken);

            return entity.Match<ActionResult<ToolAssignmentDto>>(
                toolAssignmentDto => Ok(toolAssignmentDto),
                () => NotFound());
        }

        [HttpPost]
        [ProducesResponseType(typeof(ToolAssignmentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ToolAssignmentDto>> Create(
            [FromBody] CreateToolAssignmentDto request,
            CancellationToken cancellationToken)
        {
            var command = new CreateToolAssignmentCommand
            {
                TakenDetectedToolId = request.TakenDetectedToolId,
                ToolId = request.ToolId,
                UserId = request.UserId,
                LocationId = request.TakenLocationId
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<ToolAssignmentDto>>(
                toolAssignment => CreatedAtAction(
                    nameof(GetById),
                    new { id = toolAssignment.Id.Value },
                    ToolAssignmentDto.FromDomain(toolAssignment)),
                error => error.ToObjectResult());
        }
        [HttpGet("user/{userId:guid}/tool/{toolId:guid}/search-active")]
        [ProducesResponseType(typeof(ToolAssignmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToolAssignmentDto>> GetActiveByUserAndTool(
    Guid userId,
    Guid toolId,
    CancellationToken cancellationToken)
        {
            var domainUserId = new UserId(userId);
            var domainToolId = new ToolId(toolId);

            var entity = await queries.GetActiveByUserAndToolAsync(
                domainUserId,
                domainToolId,
                cancellationToken);

            return entity.Match<ActionResult<ToolAssignmentDto>>(
                ta => Ok(ToolAssignmentDto.FromDomain(ta)),
                () => NotFound("Active assignment not found for given user & tool"));
        }

        [HttpGet("user/{userId:guid}/tool/{toolId:guid}/search")]
        [ProducesResponseType(typeof(ToolAssignmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToolAssignmentDto>> GetLastByUserAndTool(
            Guid userId,
            Guid toolId,
            CancellationToken cancellationToken)
        {
            var domainUserId = new UserId(userId);
            var domainToolId = new ToolId(toolId);

            var entity = await queries.GetLastByUserAndToolAsync(domainUserId, domainToolId, cancellationToken);

            return entity.Match<ActionResult<ToolAssignmentDto>>(
                ta => Ok(ToolAssignmentDto.FromDomain(ta)),
                () => NotFound());
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteToolAssignmentCommand
            {
                ToolAssignmentId = id
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                error => error.ToObjectResult());
        }

        [HttpPatch("{id:guid}/return")]
        [ProducesResponseType(typeof(ToolAssignmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToolAssignmentDto>> Return(
            Guid id,
            [FromBody] ReturnToolAssignmentDto request,
            CancellationToken cancellationToken)
        {
            var command = new ReturnToolAssignmentCommand
            {
                ToolAssignmentId = id,
                LocationId = request.ReturnedLocationId,
                ReturnedDetectedToolId = request.ReturnedDetectedToolId
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<ToolAssignmentDto>>(
                toolAssignment => Ok(ToolAssignmentDto.FromDomain(toolAssignment)),
                error => error.ToObjectResult());
        }


        // --- 🔥 батч create ---
        [HttpPost("batch")]
        [ProducesResponseType(typeof(IReadOnlyList<ToolAssignmentDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<ToolAssignmentDto>>> CreateBatch(
            [FromBody] CreateToolAssignmentsBatchDto request,
            CancellationToken cancellationToken)
        {
            if (request.Items is null || request.Items.Count == 0)
                return BadRequest("Items collection must not be empty.");

            var command = new CreateToolAssignmentsCommand
            {
                Items = request.Items
                    .Select(item => new CreateToolAssignmentsCommandItem
                    {
                        TakenDetectedToolId = item.TakenDetectedToolId,
                        ToolId = item.ToolId,
                        UserId = item.UserId,
                        LocationId = item.LocationId
                    })
                    .ToList()
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<IReadOnlyList<ToolAssignmentDto>>>(
                assignments =>
                {
                    var dtos = assignments
                        .Select(ToolAssignmentDto.FromDomain)
                        .ToList();

                    return StatusCode(StatusCodes.Status201Created, (IReadOnlyList<ToolAssignmentDto>)dtos);
                },
                error => error.ToObjectResult());
        }

        // --- 🔁 батч return ---
        [HttpPost("batch/return")]
        [ProducesResponseType(typeof(IReadOnlyList<ToolAssignmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<ToolAssignmentDto>>> ReturnBatch(
            [FromBody] ReturnToolAssignmentsBatchDto request,
            CancellationToken cancellationToken)
        {
            if (request.Items is null || request.Items.Count == 0)
                return BadRequest("Items collection must not be empty.");

            var command = new ReturnToolAssignmentsCommand
            {
                Items = request.Items
                    .Select(item => new ReturnToolAssignmentsCommandItem
                    {
                        ToolAssignmentId = item.ToolAssignmentId,
                        LocationId = item.LocationId,
                        ReturnedDetectedToolId = item.ReturnedDetectedToolId
                    })
                    .ToList()
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<IReadOnlyList<ToolAssignmentDto>>>(
                assignments =>
                {
                    var dtos = assignments
                        .Select(ToolAssignmentDto.FromDomain)
                        .ToList();

                    return Ok((IReadOnlyList<ToolAssignmentDto>)dtos);
                },
                error => error.ToObjectResult());
        }
    }
}