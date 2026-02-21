using Api.DTOs;
using Application.Entities.Users.Commands;
using Application.Entities.Users.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("users")]
public class AuthController(ISender sender) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> Login(
        [FromBody] LoginDto request,
        CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand
        {
            Email = request.Email.ToLower(),
            Password = request.Password,
            Longitude = request.Longitude,
            Latitude = request.Latitude

        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<UserDto>>(
            user => Ok(UserDto.FromDomain(user)),
            error => error switch
            {
                UserNotFoundException => BadRequest("User not found"),
                InvalidPasswordException => BadRequest("Invalid password"),
                InactiveUserException => BadRequest("User is inactive"),
                _ => BadRequest("Login failed")
            }
        );
    }
}

public record LoginDto(string Email, string Password, double Longitude, double Latitude);
