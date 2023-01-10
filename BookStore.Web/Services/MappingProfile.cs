using AutoMapper;
using BookStore.DataAccess.Models;
using BookStore.ViewModelData;

namespace BookStore.Web.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookVM>();
            CreateMap<BookVM, Book>();
            CreateMap<Author, AuthorVM>();
            CreateMap<AuthorVM, Author>();
            CreateMap<BookComment, BookCommentVM>();
            CreateMap<BookCommentVM, BookComment>();
            CreateMap<Publisher, PublisherVM>();
            CreateMap<PublisherVM, Publisher>();
        }
    }
}
