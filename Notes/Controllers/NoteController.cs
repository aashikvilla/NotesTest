namespace Notes.Controllers
{
    [Route($"{ApiConstants.Base}/[controller]")]
    [ApiController]
    public class NoteController : Controller
    {
        [HttpPut(ApiConstants.UpdateNote)]
        public async Task<IActionResult> UpdateNoteAsync(NoteDto noteDto)
        {
            throw new NotImplementedException();
        }
    }
}
