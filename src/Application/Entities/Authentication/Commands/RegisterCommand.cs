using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Authentication.Exceptions;
using Domain.Models.Roles;
using Domain.Models.Users;
using LanguageExt;
using MediatR;

namespace Application.Entities.Authentication.Commands
{
    public record RegisterCommand : IRequest<Either<AuthenticationException, AuthenticationResult>>
    {
        public required string FullName { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
        public Guid? RoleId { get; init; } // Optional, defaults to "User" role
    }

    public class RegisterCommandHandler(
        IUsersRepository usersRepository,
        IUsersQueries usersQueries,
        IRolesQueries rolesQueries,
        IJwtTokenGenerator jwtTokenGenerator)
        : IRequestHandler<RegisterCommand, Either<AuthenticationException, AuthenticationResult>>
    {
        public async Task<Either<AuthenticationException, AuthenticationResult>> Handle(
            RegisterCommand request,
            CancellationToken cancellationToken)
        {
            // Check if email already exists
            var existingUser = await usersQueries.GetByEmailAsync(request.Email, cancellationToken);
            
            if (existingUser.IsSome)
                return new EmailAlreadyExistsException(request.Email);

            // Get default "User" role if RoleId not provided
            RoleId roleId;
            string roleName;

            if (request.RoleId.HasValue)
            {
                roleId = new RoleId(request.RoleId.Value);
                var roleOption = await rolesQueries.GetByIdAsync(roleId, cancellationToken);
                
                if (roleOption.IsNone)
                    return new RoleNotFoundForAuthenticationException(roleId);
                
                roleName = roleOption.Match(r => r.Title, () => "User");
            }
            else
            {
                // Find default "User" role
                var defaultRole = await rolesQueries.GetByTitleAsync("user", cancellationToken);
                
                if (defaultRole.IsNone)
                    return new DefaultRoleNotFoundException();
                
                roleId = defaultRole.Match(r => r.Id, () => throw new InvalidOperationException());
                roleName = "User";
            }

            try
            {
                // Create new user
                var newUser = User.New(
                    request.FullName,
                    request.Email,
                    roleId,
                    request.Password,
                    isActive: true);

                var createdUser = await usersRepository.AddAsync(newUser, cancellationToken);

                // Generate JWT token
                var token = jwtTokenGenerator.GenerateToken(createdUser, roleName);

                return new AuthenticationResult(createdUser, token);
            }
            catch (Exception ex)
            {
                return new UnhandledAuthenticationException(ex);
            }
        }
    }
}