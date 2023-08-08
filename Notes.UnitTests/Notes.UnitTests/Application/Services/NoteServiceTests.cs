﻿using Notes.Application.Services.Notes;

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
               .With(n => n.UserId, noteDto.UserId)
               .Create();

            _noteRepositoryMock.Setup(x => x.GetNoteByIdAsync(noteDto.Id)).ReturnsAsync(note);
            _noteRepositoryMock.Setup(r => r.UpdateNoteAsync(It.IsAny<Note>())).Returns(Task.CompletedTask);

            // Act
            var result = await _noteService.UpdateNoteAsync(noteDto);

            // Assert

            _noteRepositoryMock.Verify(r => r.UpdateNoteAsync(It.Is<Note>(n => n.Id == noteDto.Id)), Times.Once);
            result.Should().BeEquivalentTo(noteDto);

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
                .With(n => n.Id, noteDto.Id)
               .With(n => n.UserId, ObjectId.GenerateNewId().ToString())
               .Create();

            _noteRepositoryMock.Setup(x => x.GetNoteByIdAsync(noteDto.Id)).ReturnsAsync(note);

            // Act
            Func<Task> act = async () => await _noteService.UpdateNoteAsync(noteDto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage(ResponseMessages.InvalidUserToUpdateNote);

        }
    }
}
