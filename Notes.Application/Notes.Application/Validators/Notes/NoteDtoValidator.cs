using FluentValidation;
using MongoDB.Bson;
using Notes.Application.Common;
using Notes.Application.Dtos.Request;

namespace Notes.Application.Validators.Notes
{
    public class NoteDtoValidator : AbstractValidator<NoteDto>
    {
        public NoteDtoValidator()
        {
            RuleFor(x => x.Id)
                .Must(IsValidObjectId).WithMessage(string.Format(ResponseMessages.InvalidErrorMessage, "{PropertyName}"));
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(string.Format(ResponseMessages.RequiredErrorMessage, "{PropertyName}"));
            RuleFor(x => x.UserId)
                .Must(IsValidObjectId).WithMessage(string.Format(ResponseMessages.InvalidErrorMessage, "{PropertyName}"))
                .WithName(nameof(NoteDto.UserId));
        }
        public bool IsValidObjectId(string id)
        {
            return ObjectId.TryParse(id, out _);
        }
    }
}
