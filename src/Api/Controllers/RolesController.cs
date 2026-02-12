using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using Application.Entities.Roles.Commands;
using Api.Modules.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("roles")]
    public class RolesController(
        IRolesQueries queries,
        IRoleControllerService service,
        ISender sender) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<RoleDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<RoleDto>>> GetRoles(
            CancellationToken cancellationToken)
        {
            var roles = await queries.GetAllAsync(cancellationToken);

            var result = roles
                .Select(r => new RoleDto(r.Id.Value, r.Title))
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoleDto>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var entity = await service.Get(id, cancellationToken);

            return entity.Match<ActionResult<RoleDto>>(
                role => Ok(new RoleDto(role.Id, role.Title)),
                () => NotFound());
        }

        [HttpPost]
        [ProducesResponseType(typeof(RoleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoleDto>> Create(
            [FromBody] CreateRoleDto request,
            CancellationToken cancellationToken)
        {
            var command = new CreateRoleCommand
            {
                Name = request.Title
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<RoleDto>>(
                role => CreatedAtAction(
                    nameof(GetById),
                    new { id = role.Id.Value },
                    new RoleDto(role.Id.Value, role.Title)),
                error => error.ToObjectResult());
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoleDto>> Update(
            Guid id,
            [FromBody] UpdateRoleDto request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateRoleCommand
            {
                RoleId = id,
                Name = request.Title
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<RoleDto>>(
                role => Ok(new RoleDto(role.Id.Value, role.Title)),
                error => error.ToObjectResult());
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteRoleCommand
            {
                RoleId = id
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                error => error.ToObjectResult());
        }
    }
}