using Notes.Domain.Entities;

namespace Notes.Domain.RepositoryInterfaces
{
    public interface INoteRepository
    {
        Task<Note> GetNoteByIdAsync(string noteId);
        Task UpdateNoteAsync(Note note);
    }
}
