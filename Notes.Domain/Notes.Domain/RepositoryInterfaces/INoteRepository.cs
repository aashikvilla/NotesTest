using Notes.Domain.Entities;

namespace Notes.Domain.RepositoryInterfaces
{
    public interface INoteRepository
    {
        Task<Note> GetByIdAsync(string noteId);
        Task UpdateAsync(Note note);
    }
}
