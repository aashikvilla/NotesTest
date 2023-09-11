using AutoMapper;
using Notes.Application.Common;
using Notes.Application.Profiles;
using Notes.Domain.Entities;

namespace Notes.IntegrationTests.Controllers
{
    public class NoteControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly IFixture _fixture;
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly IMapper _mapper;


        public NoteControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            _fixture = new Fixture();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<NoteProfile>()).CreateMapper();
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
            var noteDto = _mapper.Map<Note, NoteDto>(note, opt =>
            {
                opt.BeforeMap((src, dest) => src.Title = title);
            });


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
            var noteDto = _mapper.Map<Note, NoteDto>(note, opt =>
            {
                opt.BeforeMap((src, dest) => src.Id = ObjectId.GenerateNewId().ToString());
            });

            // Act
            var response = await _client.PutAsJsonAsync(ApiConstants.UpdateNoteEndpoint, note);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task SearchNote_WithSearchTerm_ShouldReturnFilteredNotes()
        {
            // Arrange
            Utilities.ReinitializeDb(_factory);
            var searchTerm = Utilities.searchTerm;
            var filteredNotes = Utilities.GetSeedingNotes().Where(n => n.Title.Contains(searchTerm)).ToList();
            var expectedNotes = _mapper.Map<List<Note>, List<NoteDto>>(filteredNotes);


            // Act
            var response = await _client.GetAsync(string.Format(ApiConstants.SearchNoteEndpoint, searchTerm));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var notes = await response.Content.ReadFromJsonAsync<IEnumerable<NoteDto>>();
            notes.Should().BeEquivalentTo(expectedNotes);
        }

        [Fact]
        public async Task SearchNote_WithoutSearchTerm_ShouldReturnBadRequest()
        {
            // Act
            var response = await _client.GetAsync(string.Format(ApiConstants.SearchNoteEndpoint, string.Empty));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var message = await response.Content.ReadAsStringAsync();
            message.Should().Contain(ResponseMessages.InvalidSearchTerm);
        }
    }
}
