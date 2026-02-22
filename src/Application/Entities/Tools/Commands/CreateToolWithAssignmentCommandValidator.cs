using System;
using System.IO;
using FluentValidation;

namespace Application.Entities.Tools.Commands
{
    public class CreateToolWithAssignmentCommandValidator
        : AbstractValidator<CreateToolWithAssignmentCommand>
    {
        public CreateToolWithAssignmentCommandValidator()
        {
            // 🔹 Обов’язкові GUID-и
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId є обов'язковим.");

            RuleFor(x => x.ActionTypeId)
                .NotEmpty()
                .WithMessage("ActionTypeId є обов'язковим.");

            RuleFor(x => x.PhotoTypeId)
                .NotEmpty()
                .WithMessage("PhotoTypeId є обов'язковим.");

            RuleFor(x => x.LocationId)
                .NotEmpty()
                .WithMessage("LocationId є обов'язковим.");

            RuleFor(x => x.ToolTypeId)
                .NotEmpty()
                .WithMessage("ToolTypeId є обов'язковим.");

            RuleFor(x => x.ToolStatusId)
                .NotEmpty()
                .WithMessage("ToolStatusId є обов'язковим.");

            // 🔹 Опціональні BrandId / ModelId, але якщо задані – не Guid.Empty
            RuleFor(x => x.BrandId)
                .Must(id => id is null || id.Value != Guid.Empty)
                .WithMessage("Якщо BrandId заданий, він не може бути Guid.Empty.");

            RuleFor(x => x.ModelId)
                .Must(id => id is null || id.Value != Guid.Empty)
                .WithMessage("Якщо ModelId заданий, він не може бути Guid.Empty.");

            // 🔹 SerialNumber (опціональний, але без лишньої трешової довжини)
            RuleFor(x => x.SerialNumber)
                .MaximumLength(100)
                .WithMessage("SerialNumber не може бути довшим за 100 символів.");

            // 🔹 Дані файлу
            RuleFor(x => x.OriginalName)
                .NotEmpty()
                .WithMessage("OriginalName є обов'язковим.")
                .MaximumLength(255)
                .WithMessage("OriginalName не може бути довшим за 255 символів.");

            RuleFor(x => x.FileStream)
                .NotNull()
                .WithMessage("FileStream є обов'язковим.")
                .Must(s => s != null && s.CanRead)
                .WithMessage("FileStream має бути доступним для читання.");

            // (За бажанням можна додати ще:
            // - перевірку на розширення файлу
            // - мінімальний/максимальний розмір, якщо ти де-небудь його знаєш)
        }
    }
}