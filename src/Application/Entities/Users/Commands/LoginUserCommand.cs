using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
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
            IUsersQueries queries,
            IUsersRepository usersRepository)   
            : IRequestHandler<LoginUserCommand, Either<UserException, User>>
        {
            public async Task<Either<UserException, User>> Handle(
                LoginUserCommand request,
                CancellationToken cancellationToken)
            {
                Option<User> userOption = await queries.GetByEmailAsync(request.Email, cancellationToken);

                if (userOption.IsNone)
                {
                    return new UserNotFoundException();
                }

                var user = userOption.Match(
                    some => some,
                    () => null!);

                if (!user.VerifyPassword(request.Password))
                    return new InvalidPasswordException(user.Id);

                if (!user.IsActive)
                    return new InactiveUserException(user.Id);
                user.UpdateLocation(request.Longitude, request.Latitude);
                await usersRepository.UpdateAsync(user, cancellationToken);

                return user;
            }
        }
    }
}