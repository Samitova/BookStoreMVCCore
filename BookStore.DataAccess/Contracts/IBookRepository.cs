using BookStore.DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Contracts
{
    public interface IBookRepository:IRepositoryBase<Book>
    {     
        Task<IEnumerable<Book>> SearchByIsbnAsync(string isbn);
        Task<IEnumerable<Book>> SearchByTitleAndAuthorAsync(string query);
        IEnumerable<Book> SearchByIsbn(string isbn);
        IEnumerable<Book> SearchByTitleAndAuthor(string query);

    }
}
