using Notes.Application.Dtos.Request;

namespace Notes.Application.Services.Notes
{
    public interface INoteService
    {
        Task<NoteDto> UpdateAsync(NoteDto noteDto);
    }
}
