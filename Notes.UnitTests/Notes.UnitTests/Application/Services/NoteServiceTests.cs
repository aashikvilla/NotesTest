using Notes.Application.Services.Notes;

namespace Notes.UnitTests.Application.Services
{
    public class NoteServiceTests
    {
        private readonly Mock<INoteRepository> _noteRepositoryMock;
        private readonly INoteService _noteService;
        private readonly Fixture _fixture;

        public NoteServiceTests()
        {
            _fixture = new Fixture();
            _noteRepositoryMock = new Mock<INoteRepository>();
            _noteService = new NoteService();
        }

        [Fact]
        public async Task UpdateNoteAsync_WhenNoteIsUpdatedSuccessfully_ShouldReturnNote()
        {
            // Arrange
            var noteDto = _fixture.Create<NoteDto>();
            var note = _fixture.Build<Note>()
               .With(n => n.Id, noteDto.Id)
               .With(n => n.Title, noteDto.Title)
               .With(n => n.Description, noteDto.Description)
               .With(n => n.Status, noteDto.Status)
               .With(n => n.Priority, noteDto.Priority)
               .Create();

            _noteRepositoryMock.Setup(x => x.GetNoteByIdAsync(note.Id)).ReturnsAsync(note);

            // Act
            var result = await _noteService.UpdateNoteAsync(noteDto);

            // Assert
            _noteRepositoryMock.Verify(r => r.UpdateNoteAsync(note), Times.Once);
            result.Should().BeEquivalentTo(note);

        }

        [Fact]
        public async Task UpdateNoteAsync_WhenNoteDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var noteDto = _fixture.Create<NoteDto>();

            _noteRepositoryMock.Setup(x => x.GetNoteByIdAsync(noteDto.Id))
                .ReturnsAsync((Note)null);

            // Act
            Func<Task> act = async () => await _noteService.UpdateNoteAsync(noteDto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage(ResponseMessages.NoteNotFound);
        }

        [Fact]
        public async Task UpdateNoteAsync_WithInvalidUserId_ShouldThrowException()
        {
            // Arrange
            var noteDto = _fixture.Create<NoteDto>();
            var note = _fixture.Build<Note>()
               .With(n => n.UserId, ObjectId.GenerateNewId().ToString())
               .Create();

            _noteRepositoryMock.Setup(x => x.GetNoteByIdAsync(note.Id)).ReturnsAsync(note);

            // Act
            Func<Task> act = async () => await _noteService.UpdateNoteAsync(noteDto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage(ResponseMessages.InvalidUserToUpdateNote);

        }
    }
}
