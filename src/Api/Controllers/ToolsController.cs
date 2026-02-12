using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using Application.Entities.Tools.Commands;
using Api.Modules.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("tools")]
    public class ToolsController(
        IToolsQueries queries,
        IToolControllerService service,
        ISender sender) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<ToolDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ToolDto>>> GetTools(
            CancellationToken cancellationToken)
        {
            var tools = await queries.GetAllAsync(cancellationToken);

            var result = tools
                .Select(ToolDto.FromDomain)
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ToolDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToolDto>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var entity = await service.Get(id, cancellationToken);

            return entity.Match<ActionResult<ToolDto>>(
                toolDto => Ok(toolDto),
                () => NotFound());
        }

        [HttpPost]
        [ProducesResponseType(typeof(ToolDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ToolDto>> Create(
            [FromBody] CreateToolDto request,
            CancellationToken cancellationToken)
        {
            var command = new CreateToolCommand
            {
                ToolTypeId = request.ToolTypeId,
                BrandId = request.BrandId,
                ModelId = request.ModelId,
                ToolStatusId = request.ToolStatusId,
                SerialNumber = request.SerialNumber
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<ToolDto>>(
                tool => CreatedAtAction(
                    nameof(GetById),
                    new { id = tool.Id.Value },
                    ToolDto.FromDomain(tool)),
                error => error.ToObjectResult());
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ToolDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToolDto>> Update(
            Guid id,
            [FromBody] UpdateToolDto request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateToolCommand
            {
                ToolId = id,
                BrandId = request.BrandId,
                ModelId = request.ModelId,
                SerialNumber = request.SerialNumber
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<ToolDto>>(
                tool => Ok(ToolDto.FromDomain(tool)),
                error => error.ToObjectResult());
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteToolCommand
            {
                ToolId = id
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                error => error.ToObjectResult());
        }

        [HttpPatch("{id:guid}/change-status")]
        [ProducesResponseType(typeof(ToolDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToolDto>> ChangeStatus(
            Guid id,
            [FromBody] ChangeToolStatusDto request,
            CancellationToken cancellationToken)
        {
            var command = new ChangeToolStatusCommand
            {
                ToolId = id,
                ToolStatusId = request.ToolStatusId
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<ToolDto>>(
                tool => Ok(ToolDto.FromDomain(tool)),
                error => error.ToObjectResult());
        }
    }
}