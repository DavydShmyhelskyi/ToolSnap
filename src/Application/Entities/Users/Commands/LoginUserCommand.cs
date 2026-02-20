using Application.Common.Interfaces.Queries;
using Application.Entities.Users.Exceptions;
using Domain.Models.Users;
using LanguageExt;
using MediatR;

namespace Application.Entities.Users.Commands
{
    public record LoginUserCommand : IRequest<Either<UserException, User>>
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
        public required double Latitude { get; init; }
        public required double Longitude { get; init; }

        public class LoginUserCommandHandler(
            IUsersQueries queries)
            : IRequestHandler<LoginUserCommand, Either<UserException, User>>
        {
            public async Task<Either<UserException, User>> Handle(
                LoginUserCommand request,
                CancellationToken cancellationToken)
            {
                // Знаходимо користувача по email
                var userOption = await queries.GetByEmailAsync(request.Email, cancellationToken);

                return await userOption.MatchAsync(
                    user =>
                    {
                        // Перевірка паролю
                        if (!user.VerifyPassword(request.Password))
                            return Task.FromResult<Either<UserException, User>>(
                                new InvalidPasswordException(user.Id)
                            );

                        if (!user.IsActive)
                            return Task.FromResult<Either<UserException, User>>(
                                new InactiveUserException(user.Id)
                            );
                        user.UpdateLocation(request.Longitude, request.Latitude);
                        return Task.FromResult<Either<UserException, User>>(user);
                    },
                    () => Task.FromResult<Either<UserException, User>>(new UserNotFoundException())
                );
            }
        }
    }
}
