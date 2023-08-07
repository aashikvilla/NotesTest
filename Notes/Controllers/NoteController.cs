namespace Notes.Controllers
{
    [Route($"{ApiConstants.Base}/[controller]")]
    [ApiController]
    public class NoteController : Controller
    {
        [HttpPut(ApiConstants.UpdateNote)]
        public IActionResult UpdateNote(NoteDto noteDto)
        {
            throw new NotImplementedException();
        }
    }
}
