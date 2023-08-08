using Notes.Application.Common;

namespace Notes.Application.Validators
{
    public class ValidationMessages
    {
        public static string PropertyName = "{PropertyName}";
        public static string RequiredErrorMessage = string.Format(ResponseMessages.RequiredErrorMessage, PropertyName);
        public static string InvalidErrorMessage = string.Format(ResponseMessages.InvalidErrorMessage, PropertyName);
    }
}
