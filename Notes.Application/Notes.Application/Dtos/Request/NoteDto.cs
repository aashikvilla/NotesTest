namespace Notes.Application.Dtos.Request
{
    public record NoteDto
    {
        public string Id { get; init; }

        public string Title { get; init; }

        public string Description { get; init; }

        public string Priority { get; init; }

        public string Status { get; init; }

        public string UserId { get; init; }
    }
}
