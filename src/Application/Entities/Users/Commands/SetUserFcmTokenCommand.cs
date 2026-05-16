using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Users.Exceptions;
using Domain.Models.Users;
using FluentValidation;
using LanguageExt;
using MediatR;

namespace Application.Entities.Users.Commands
{
    public record SetUserFcmTokenCommand : IRequest<Either<UserException, User>>
    {
        public required Guid UserId { get; init; }
        public string? FcmToken { get; init; }
    }

    public class SetUserFcmTokenCommandHandler(
        IUsersQueries queries,
        IUsersRepository repository)
        : IRequestHandler<SetUserFcmTokenCommand, Either<UserException, User>>
    {
        public async Task<Either<UserException, User>> Handle(
            SetUserFcmTokenCommand request,
            CancellationToken cancellationToken)
        {
            var userId = new UserId(request.UserId);
            var userOpt = await queries.GetByIdAsync(userId, cancellationToken);

            if (userOpt.IsNone)
                return new UserNotFoundException();

            return await userOpt.MatchAsync(
                async user =>
                {
                    user.SetFcmToken(request.FcmToken);
                    return (Either<UserException, User>)await repository.UpdateAsync(user, cancellationToken);
                },
                () => new UserNotFoundException());
        }
    }

    public class SetUserFcmTokenCommandValidator : AbstractValidator<SetUserFcmTokenCommand>
    {
        public SetUserFcmTokenCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
