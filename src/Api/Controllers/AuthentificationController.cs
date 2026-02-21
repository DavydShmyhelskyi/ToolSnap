using Api.DTOs;
using Api.Modules.Errors;
using Application.Entities.Authentication.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthentificationController(ISender sender) : ControllerBase
    {
        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthenticationResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AuthenticationResponseDto>> Register(
            [FromBody] RegisterDto request,
            CancellationToken cancellationToken)
        {
            var command = new RegisterCommand
            {
                FullName = request.FullName,
                Email = request.Email,
                Password = request.Password,
                RoleId = request.RoleId
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<AuthenticationResponseDto>>(
                authResult => CreatedAtAction(
                    nameof(GetProfile),
                    new { id = authResult.User.Id.Value },
                    new AuthenticationResponseDto(
                        authResult.User.Id.Value,
                        authResult.User.FullName,
                        authResult.User.Email,
                        authResult.User.Role?.Title ?? "User",
                        authResult.User.IsActive,
                        authResult.User.ConfirmedEmail,
                        authResult.Token)),
                error => error.ToObjectResult());
        }

        /// <summary>
        /// Login user
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthenticationResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<AuthenticationResponseDto>> Login(
            [FromBody] LoginDto request,
            CancellationToken cancellationToken)
        {
            var command = new LoginCommand
            {
                Email = request.Email,
                Password = request.Password
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<AuthenticationResponseDto>>(
                authResult => Ok(new AuthenticationResponseDto(
                    authResult.User.Id.Value,
                    authResult.User.FullName,
                    authResult.User.Email,
                    authResult.User.Role?.Title ?? "User",
                    authResult.User.IsActive,
                    authResult.User.ConfirmedEmail,
                    authResult.Token)),
                error => error.ToObjectResult());
        }

        /// <summary>
        /// Get current user profile (requires authentication)
        /// </summary>
        [HttpGet("profile")]
        [Authorize]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<object> GetProfile()
        {
            // Get user info from JWT claims
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var fullName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            return Ok(new
            {
                UserId = userId,
                Email = email,
                FullName = fullName,
                Role = role,
                Claims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }
    }
}