using AutoMapper;
using Notes.Application.Dtos.Request;
using Notes.Domain.Entities;

namespace Notes.Application.Profiles
{
    public class NoteProfile : Profile
    {
        public NoteProfile()
        {
            CreateMap<Note, NoteDto>().ReverseMap();
        }
    }
}
