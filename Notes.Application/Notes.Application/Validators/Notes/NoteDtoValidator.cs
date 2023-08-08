using FluentValidation;
using MongoDB.Bson;
using Notes.Application.Dtos.Request;

namespace Notes.Application.Validators.Notes
{
    public class NoteDtoValidator : AbstractValidator<NoteDto>
    {
        public NoteDtoValidator()
        {
            RuleFor(x => x.Id)
                .Must(IsValidObjectId).WithMessage(ValidationMessages.InvalidErrorMessage);
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(ValidationMessages.RequiredErrorMessage);
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(ValidationMessages.RequiredErrorMessage);
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage(ValidationMessages.RequiredErrorMessage);
            RuleFor(x => x.Priority)
                .NotEmpty().WithMessage(ValidationMessages.RequiredErrorMessage);
            RuleFor(x => x.UserId)
                .Must(IsValidObjectId).WithMessage(ValidationMessages.InvalidErrorMessage)
                .WithName(nameof(NoteDto.UserId));
        }
        public bool IsValidObjectId(string id)
        {
            return ObjectId.TryParse(id, out _);
        }
    }
}
