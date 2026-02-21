using Application.Entities.PhotoSessions.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Entities.PhotoForDetections.Commands
{
    public class GetPhotoFilesBySessionCommandValidator : AbstractValidator<GetPhotoFilesBySessionCommand>
    {
        public GetPhotoFilesBySessionCommandValidator()
        {
            RuleFor(x => x.PhotoSessionId)
                .NotEmpty().WithMessage("Photo session ID is required.");
        }
    }
}
