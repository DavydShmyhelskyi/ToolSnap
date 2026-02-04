using Domain.Models.PhotoSessions;

namespace Api.DTOs
{
    public record ActionTypeDto(
        Guid Id,
        string Title)
    {
        public static ActionTypeDto FromDomain(ActionType actionType) =>
            new(
                actionType.Id.Value,
                actionType.Title);
    }

    public record CreateActionTypeDto(
        string Title);

    public record UpdateActionTypeDto(
        string Title);
}