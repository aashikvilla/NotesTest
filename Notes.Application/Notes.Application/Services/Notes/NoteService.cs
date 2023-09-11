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

        public async Task<NoteDto> UpdateAsync(NoteDto noteDto)
        {
            var existingNote = await _noteRepository.GetByIdAsync(noteDto.Id);
            if (existingNote == null)
            {
                throw new InvalidOperationException(ResponseMessages.NoteNotFound);
            }
            if (noteDto.UserId != existingNote.UserId)
            {
                throw new InvalidOperationException(ResponseMessages.InvalidUserToUpdateNote);
            }

            var note = _mapper.Map<NoteDto, Note>(noteDto);
            await _noteRepository.UpdateAsync(note);

            return noteDto;
        }

        public async Task<List<NoteDto>> SearchAsync(string searchTerm)
        {
            var notes = await _noteRepository.SearchAsync(searchTerm);
            return _mapper.Map<List<Note>, List<NoteDto>>(notes);
        }
    }
}
