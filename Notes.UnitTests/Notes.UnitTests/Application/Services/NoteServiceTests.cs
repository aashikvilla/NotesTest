using Notes.Application.Dtos.Request;
using Notes.Application.Profiles;
using Notes.Application.Services.Notes;
using Notes.Domain.Entities;
using Notes.Domain.RepositoryInterfaces;

namespace Notes.UnitTests.Application.Services
{
    public class NoteServiceTests
    {
        private readonly Mock<INoteRepository> _noteRepositoryMock;
        private readonly INoteService _noteService;
        private readonly Fixture _fixture;
        private readonly IMapper _mapper;

        public NoteServiceTests()
        {
            _fixture = new Fixture();
            _noteRepositoryMock = new Mock<INoteRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<NoteProfile>();
            });
            _mapper = config.CreateMapper();

            _noteService = new NoteService(_noteRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task UpdateAsync_WhenNoteIsUpdatedSuccessfully_ShouldReturnNote()
        {
            // Arrange
            var noteDto = _fixture.Create<NoteDto>();
            var note = _fixture.Build<Note>()
               .With(n => n.Id, noteDto.Id)
               .With(n => n.Title, noteDto.Title)
               .With(n => n.Description, noteDto.Description)
               .With(n => n.Status, noteDto.Status)
               .With(n => n.Priority, noteDto.Priority)
               .With(n => n.UserId, noteDto.UserId)
               .Create();

            _noteRepositoryMock.Setup(x => x.GetByIdAsync(noteDto.Id)).ReturnsAsync(note);
            _noteRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Note>())).Returns(Task.CompletedTask);

            // Act
            var result = await _noteService.UpdateAsync(noteDto);

            // Assert

            _noteRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Note>(n => n.Id == noteDto.Id)), Times.Once);
            result.Should().BeEquivalentTo(noteDto);

        }

        [Fact]
        public async Task UpdateAsync_WhenNoteDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var noteDto = _fixture.Create<NoteDto>();

            _noteRepositoryMock.Setup(x => x.GetByIdAsync(noteDto.Id))
                .ReturnsAsync((Note)null);

            // Act
            Func<Task> act = async () => await _noteService.UpdateAsync(noteDto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage(ResponseMessages.NoteNotFound);
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidUserId_ShouldThrowException()
        {
            // Arrange
            var noteDto = _fixture.Create<NoteDto>();
            var note = _fixture.Build<Note>()
                .With(n => n.Id, noteDto.Id)
               .With(n => n.UserId, ObjectId.GenerateNewId().ToString())
               .Create();

            _noteRepositoryMock.Setup(x => x.GetByIdAsync(noteDto.Id)).ReturnsAsync(note);

            // Act
            Func<Task> act = async () => await _noteService.UpdateAsync(noteDto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage(ResponseMessages.InvalidUserToUpdateNote);

        }
    }
}
