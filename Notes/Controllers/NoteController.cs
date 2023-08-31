using Notes.Application.Common;
using Notes.Application.Services.Notes;

namespace Notes.Controllers
{
    [Route($"{ApiConstants.Base}/{ApiConstants.NoteController}")]
    [ApiController]
    public class NoteController : Controller
    {
        private readonly INoteService _noteService;
        private readonly IValidator<NoteDto> _noteDtoValidator;


        public NoteController(INoteService noteService, IValidator<NoteDto> noteDtoValidator)
        {
            _noteService = noteService;
            _noteDtoValidator = noteDtoValidator;
        }


        [HttpPut(ApiConstants.UpdateNote)]
        public async Task<IActionResult> UpdateNoteAsync(NoteDto noteDto)
        {
            var validationResult = _noteDtoValidator.Validate(noteDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }
            return Ok(await _noteService.UpdateAsync(noteDto));
        }

        [HttpGet]
        public async Task<IActionResult> SearchNoteAsync([FromQuery] string searchTerm = "")
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return BadRequest(ResponseMessages.InvalidSearchTerm);
            }
            return Ok(await _noteService.SearchAsync(searchTerm));
        }
    }
}
