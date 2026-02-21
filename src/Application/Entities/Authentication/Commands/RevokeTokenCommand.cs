using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Authentication.Exceptions;
using LanguageExt;
using MediatR;

namespace Application.Entities.Authentication.Commands
{
    public record RevokeTokenCommand : IRequest<Either<AuthenticationException, LanguageExt.Unit>>
    {
        public required string RefreshToken { get; init; }
    }

    public class RevokeTokenCommandHandler(
        IUsersQueries usersQueries,
        IUsersRepository usersRepository)
        : IRequestHandler<RevokeTokenCommand, Either<AuthenticationException, LanguageExt.Unit>>
    {
        public async Task<Either<AuthenticationException, LanguageExt.Unit>> Handle(
            RevokeTokenCommand request,
            CancellationToken cancellationToken)
        {
            // Find user by refresh token (with tracking)
            var user = await usersQueries.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);

            if (user == null)
                return new InvalidRefreshTokenException();

            // Revoke refresh token (logout)
            user.RevokeRefreshToken();
            
            // Save changes (user already tracked)
            await usersRepository.SaveChangesAsync(cancellationToken);

            return LanguageExt.Unit.Default;
        }
    }
}