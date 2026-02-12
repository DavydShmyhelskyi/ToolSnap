using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using Application.Entities.Users.Commands;
using Api.Modules.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController(
        IUsersQueries queries,
        IUserControllerService service,
        ISender sender) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<UserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<UserDto>>> GetUsers(
            CancellationToken cancellationToken)
        {
            var users = await queries.GetAllAsync(cancellationToken);

            var result = users
                .Select(UserDto.FromDomain)
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var entity = await service.Get(id, cancellationToken);

            return entity.Match<ActionResult<UserDto>>(
                userDto => Ok(userDto),
                () => NotFound());
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> Create(
            [FromBody] CreateUserDto request,
            CancellationToken cancellationToken)
        {
            var command = new CreateUserCommand
            {
                FullName = request.FullName,
                Email = request.Email,
                RoleId = request.RoleId,
                Password = request.Password,
                IsActive = request.IsActive
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<UserDto>>(
                user => CreatedAtAction(
                    nameof(GetById),
                    new { id = user.Id.Value },
                    UserDto.FromDomain(user)),
                error => error.ToObjectResult());
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Update(
            Guid id,
            [FromBody] UpdateUserDto request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateUserCommand
            {
                UserId = id,
                FullName = request.FullName,
                Email = request.Email,
                RoleId = request.RoleId
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<UserDto>>(
                user => Ok(UserDto.FromDomain(user)),
                error => error.ToObjectResult());
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteUserCommand
            {
                UserId = id
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                error => error.ToObjectResult());
        }

        [HttpPatch("{id:guid}/confirm-email")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> ConfirmEmail(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new ConfirmEmailCommand
            {
                UserId = id
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<UserDto>>(
                user => Ok(UserDto.FromDomain(user)),
                error => error.ToObjectResult());
        }

        [HttpPatch("{id:guid}/activate")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Activate(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new ActivateUserCommand
            {
                UserId = id
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<UserDto>>(
                user => Ok(UserDto.FromDomain(user)),
                error => error.ToObjectResult());
        }

        [HttpPatch("{id:guid}/deactivate")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Deactivate(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeactivateUserCommand
            {
                UserId = id
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<UserDto>>(
                user => Ok(UserDto.FromDomain(user)),
                error => error.ToObjectResult());
        }

        [HttpPatch("{id:guid}/change-password")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> ChangePassword(
            Guid id,
            [FromBody] ChangePasswordDto request,
            CancellationToken cancellationToken)
        {
            var command = new ChangeUserPasswordCommand
            {
                UserId = id,
                CurrentPassword = request.CurrentPassword,
                NewPassword = request.NewPassword
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<UserDto>>(
                user => Ok(UserDto.FromDomain(user)),
                error => error.ToObjectResult());
        }
    }
}