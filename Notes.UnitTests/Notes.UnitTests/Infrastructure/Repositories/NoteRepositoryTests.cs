using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Notes.Domain.Entities;
using Notes.Infrastructure.Data;
using Notes.Infrastructure.Repositories;

namespace Notes.UnitTests.Infrastructure.Repositories
{

    public class NoteRepositoryTests
    {
        private readonly Mock<IMongoDatabase> _mongoDatabaseMock;
        private readonly Mock<IMongoCollection<Note>> _mongoCollectionMock;
        private readonly Mock<IAsyncCursor<Note>> _mongoCursorMock;
        private readonly NoteRepository _noteRepository;
        private readonly IFixture _fixture;

        public NoteRepositoryTests()
        {
            _mongoDatabaseMock = new Mock<IMongoDatabase>();
            _mongoCollectionMock = new Mock<IMongoCollection<Note>>();
            _mongoCursorMock = new Mock<IAsyncCursor<Note>>();

            var settings = new MongoDbSettings
            {
                NotesCollectionName = "Notes"
            };

            var mockOptions = new Mock<IOptions<MongoDbSettings>>();
            mockOptions.Setup(ap => ap.Value).Returns(settings);

            _mongoDatabaseMock.Setup(db => db.GetCollection<Note>(It.IsAny<string>(), null))
                .Returns(_mongoCollectionMock.Object);

            _noteRepository = new NoteRepository(_mongoDatabaseMock.Object, mockOptions.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public void GetNoteByIdAsync_ShouldUseCorrectFilter()
        {
            // Arrange
            var noteId = _fixture.Create<string>();
            var serializerRegistry = BsonSerializer.SerializerRegistry;
            var documentSerializer = serializerRegistry.GetSerializer<Note>();
            var expectedFilter = Builders<Note>.Filter.Eq(n => n.Id, noteId);

            _mongoCollectionMock.Setup(m => m.FindAsync(It.IsAny<FilterDefinition<Note>>(), null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_mongoCursorMock.Object);

            // Act
            _noteRepository.GetByIdAsync(noteId);

            // Assert
            _mongoCollectionMock.Verify(col => col.FindAsync(
                It.Is<FilterDefinition<Note>>(filter => filter.Render(documentSerializer, serializerRegistry) == expectedFilter.Render(documentSerializer, serializerRegistry)),
                null,
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void test()
        {
            // Arrange
            var noteId = _fixture.Create<string>();

            _mongoCollectionMock.Setup(m => m.FindAsync(It.IsAny<FilterDefinition<Note>>(), null, It.IsAny<CancellationToken>()))
                .ReturnsAsync((IAsyncCursor<Note>)null);

            // Act
            _noteRepository.GetByIdAsync(noteId);

            // Assert
            _mongoCollectionMock.Verify(col => col.FindAsync(
                It.IsAny<FilterDefinition<Note>>(),
                null,
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }


}
