using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Notes.Domain.Entities;
using Notes.Domain.RepositoryInterfaces;
using Notes.Infrastructure.Data;

namespace Notes.Infrastructure.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly IMongoCollection<Note> _notes;
        public NoteRepository(IMongoDatabase mongoDatabase, IOptions<MongoDbSettings> mongoDbSettings)
        {
            _notes = mongoDatabase.GetCollection<Note>(
                mongoDbSettings.Value.NotesCollectionName);
        }
        public async Task<Note> GetByIdAsync(string noteId)
        {
            var filter = Builders<Note>.Filter.Eq(note => note.Id, noteId);
            return await _notes.Find(filter).FirstOrDefaultAsync();
        }
        public async Task UpdateAsync(Note note)
        {
            var filter = Builders<Note>.Filter.Eq(n => n.Id, note.Id);
            await _notes.ReplaceOneAsync(filter, note);
        }

        public async Task<List<Note>> SearchAsync(string searchTerm)
        {
            return await _notes.Aggregate()
                .Search(Builders<Note>.Search.Text(w => w.Title, searchTerm))
                .ToListAsync();
        }
    }
}
