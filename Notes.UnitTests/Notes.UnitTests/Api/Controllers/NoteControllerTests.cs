namespace Notes.UnitTests.Api.Controllers
{
    public class NoteControllerTests
    {
        private NoteController _noteController;
        private Fixture _fixture;

        public NoteControllerTests()
        {
            _noteController = new NoteController();
            _fixture = new Fixture();
        }

        [Fact]
        public async Task UpdateNote_ShouldReturnUpdatedNote_WhenUpdateIsSuccessful()
        {
            // Arrange
            var note = _fixture.Build<NoteDto>()
                .With(n => n.Id, ObjectId.GenerateNewId().ToString())
                .With(n => n.UserId, ObjectId.GenerateNewId().ToString())
                .Create();

            // Act
            var result = await _noteController.UpdateNoteAsync(note);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(note);
        }

        [Fact]
        public async Task UpdateNote_ShouldReturnBadRequest_WhenModelStateIsInValid()
        {
            // Arrange
            var note = new NoteDto
            {
                Id = string.Empty
            };
            var expectedErrors = new string[]
            {
                "Invalid Note Id"
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
