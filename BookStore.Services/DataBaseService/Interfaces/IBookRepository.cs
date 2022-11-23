using BookStore.Data.Models.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Data.Models.Interfaces
{
    public interface IBookRepository:IRepositoryBase<BookDTO>
    {
        Task<BookDTO> GetByTitleAsync(string title);
        Task<BookDTO> GetByAuthorAsync(string author);
        Task<IEnumerable<BookDTO>> SearchByIsbnAsync(string isbn);
        Task<IEnumerable<BookDTO>> SearchByTitleAndAuthorAsync(string query);
        IEnumerable<BookDTO> SearchByIsbn(string isbn);
        IEnumerable<BookDTO> SearchByTitleAndAuthor(string query);

    }
}
