namespace Notes.IntegrationTests.Controllers
{
    public class NoteControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly IFixture _fixture;
        private readonly CustomWebApplicationFactory<Program> _factory;


        public NoteControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            _fixture = new Fixture();
        }


        [Fact]
        public async Task UpdateNote_ShouldNotReturnNotFound()
        {
            // Arrange
            var noteDto = _fixture.Create<NoteDto>();

            // Act
            var response = await _client.PutAsJsonAsync(ApiConstants.UpdateNoteEndpoint, noteDto);

            // Assert
            response.StatusCode.Should().NotBe(HttpStatusCode.NotFound);

        }

        [Fact]
        public async Task UpdateNote_WhenNoteIsValid_ShouldReturnNote()
        {
            // Arrange
            Utilities.ReinitializeDb(_factory);
            var note = Utilities.GetSeedingNotes().FirstOrDefault();
            var title = _fixture.Create<string>();
            note.Title = title;


            // Act
            var response = await _client.PutAsJsonAsync(ApiConstants.UpdateNoteEndpoint, note);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var updatedNote = await response.Content.ReadFromJsonAsync<NoteDto>();

            //check if title is updated correctly
            updatedNote.Title.Should().BeEquivalentTo(title);
        }

        [Fact]
        public async Task UpdateNote_WhenNoteIdIsInValid_ShouldReturnBadRequest()
        {
            // Arrange
            Utilities.ReinitializeDb(_factory);
            var note = Utilities.GetSeedingNotes().FirstOrDefault();
            note.Id = BsonObjectId.GenerateNewId().ToString();

            // Act
            var response = await _client.PutAsJsonAsync(ApiConstants.UpdateNoteEndpoint, note);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
