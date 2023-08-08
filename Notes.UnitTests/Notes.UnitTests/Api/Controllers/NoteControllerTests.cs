using Notes.Application.Services.Notes;
using Notes.Application.Validators.Notes;

namespace Notes.UnitTests.Api.Controllers
{
    public class NoteControllerTests
    {
        private NoteController _noteController;
        private Fixture _fixture;
        private Mock<INoteService> _noteServiceMock;
        private readonly IValidator<NoteDto> _noteDtoValidator;

        public NoteControllerTests()
        {
            _fixture = new Fixture();
            _noteServiceMock = new Mock<INoteService>();
            _noteDtoValidator = new NoteDtoValidator();
            _noteController = new NoteController(_noteServiceMock.Object, _noteDtoValidator);
        }

        [Fact]
        public async Task UpdateNote_ShouldReturnUpdatedNote_WhenUpdateIsSuccessful()
        {
            // Arrange
            var note = _fixture.Build<NoteDto>()
                .With(n => n.Id, ObjectId.GenerateNewId().ToString())
                .With(n => n.UserId, ObjectId.GenerateNewId().ToString())
                .Create();
            _noteServiceMock.Setup(s => s.UpdateNoteAsync(note)).ReturnsAsync(note);
            // Act
            var result = await _noteController.UpdateNoteAsync(note);

            // Assert
            _noteServiceMock.Verify(s => s.UpdateNoteAsync(note), Times.Once);
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(note);
        }

        [Fact]
        public async Task UpdateNote_ShouldReturnBadRequest_WhenModelStateIsInValid()
        {
            // Arrange
            var note = new NoteDto();

            var expectedErrors = new string[]
            {
                string.Format(ResponseMessages.InvalidErrorMessage, nameof(NoteDto.Id)),
                string.Format(ResponseMessages.InvalidErrorMessage, nameof(NoteDto.UserId)),
                string.Format(ResponseMessages.RequiredErrorMessage, nameof(NoteDto.Title)),
                string.Format(ResponseMessages.RequiredErrorMessage, nameof(NoteDto.Description)),
                string.Format(ResponseMessages.RequiredErrorMessage, nameof(NoteDto.Priority)),
                string.Format(ResponseMessages.RequiredErrorMessage, nameof(NoteDto.Status))
            };

            // Act
            var result = await _noteController.UpdateNoteAsync(note);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = (result as BadRequestObjectResult).Value as IEnumerable<string>;
            errors.Should().BeEquivalentTo(expectedErrors);
        }
    }
}
