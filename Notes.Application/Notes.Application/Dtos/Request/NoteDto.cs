﻿namespace Notes.Application.Dtos.Request
{
    public record NoteDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Priority { get; set; }

        public string Status { get; set; }

        public string UserId { get; set; }
    }
}
