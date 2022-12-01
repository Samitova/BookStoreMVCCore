using AutoMapper;
using BookStore.Data.Models.ModelsDTO;
using BookStore.Data.Models.ViewModels;

namespace BookStore.Web.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookDTO, BookVM>();
            CreateMap<BookVM, BookDTO>();
            CreateMap<AuthorDTO, AuthorVM>();
            CreateMap<AuthorVM, AuthorDTO>();
            CreateMap<BookCommentDTO, BookCommentVM>();
            CreateMap<BookCommentVM, BookCommentDTO>();
            
        }
    }
}
