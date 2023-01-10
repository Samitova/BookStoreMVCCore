using BookStore.DataAccess.Contracts;
using BookStore.DataAccess.DataContext;
using BookStore.DataAccess.Models;

namespace BookStore.DataAccess.Repositories
{
    public class BookCommentsRepository : RepositoryBase<BookComment>, IBookCommentsRepository
    {
        public BookCommentsRepository(BookStoreContext context) : base(context)
        {
        }
    }
}
