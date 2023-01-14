using AutoMapper;
using BookStore.DataAccess.Models;
using BookStore.ViewModelData;

namespace BookStore.Web.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookViewModel>();
            CreateMap<BookViewModel, Book>();
            CreateMap<Author, AuthorViewModel>();
            CreateMap<AuthorViewModel, Author>();
            CreateMap<BookComment, BookCommentViewModel>();
            CreateMap<BookCommentViewModel, BookComment>();
            CreateMap<Publisher, PublisherViewModel>();
            CreateMap<PublisherViewModel, Publisher>();
        }
    }
}
