using AutoMapper;
using Notes.Application.Common;
using Notes.Application.Dtos.Request;
using Notes.Domain.Entities;
using Notes.Domain.RepositoryInterfaces;

namespace Notes.Application.Services.Notes
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly IMapper _mapper;

        public NoteService(INoteRepository noteRepository, IMapper mapper)
        {
            _noteRepository = noteRepository;
            _mapper = mapper;
        }

        public async Task<NoteDto> UpdateNoteAsync(NoteDto noteDto)
        {
            var existingNote = await _noteRepository.GetNoteByIdAsync(noteDto.Id);
            if (existingNote == null)
            {
                throw new InvalidOperationException(ResponseMessages.NoteNotFound);
            }
            if (noteDto.UserId != existingNote.UserId)
            {
                throw new InvalidOperationException(ResponseMessages.InvalidUserToUpdateNote);
            }

            var note = _mapper.Map<NoteDto, Note>(noteDto);
            await _noteRepository.UpdateNoteAsync(note);

            return noteDto;
        }
    }
}
