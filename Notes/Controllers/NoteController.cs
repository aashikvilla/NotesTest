using Microsoft.AspNetCore.Mvc;
using Notes.Application.Dtos.Request;

namespace Notes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : Controller
    {
        [HttpPut("UpdateNote")]
        public IActionResult UpdateNote(NoteDto noteDto)
        {
            throw new NotImplementedException();
        }
    }
}
