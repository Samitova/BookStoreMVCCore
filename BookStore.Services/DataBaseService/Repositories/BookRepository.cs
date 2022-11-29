using BookStore.Data.Models.Interfaces;
using BookStore.Data.Models.ModelsDTO;
using BookStore.Services.DataBaseService.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BookStore.Services.DataBaseService.Repositories
{
    public class BookRepository : RepositoryBase<BookDTO>, IBookRepository
    {

        public BookRepository(BookStoreContext context) : base(context)
        { }

        public  Task<BookDTO> GetByTitleAsync(string title)
        {
            return   FirstOrDefaultAsync(w => w.Title  == title);
        }

        public  Task<BookDTO> GetByAuthorAsync(string author)
        {
            return  FirstOrDefaultAsync(w => w.AuthorFullName == author);
        }

        public Task<IEnumerable<BookDTO>> SearchByIsbnAsync(string isbn)
        {            
            return GetAllAsync(filter: x => x.ISBN==isbn);
            
        }       

        public  Task<IEnumerable<BookDTO>> SearchByTitleAndAuthorAsync(string query)
        {
            return  GetAllAsync(filter: x => x.Title.ToLower().Contains(query.ToLower()) 
                                        || x.AuthorFullName.ToLower().Contains(query.ToLower()));
        }

        public IEnumerable<BookDTO> SearchByIsbn(string isbn)
        {
            return GetAll(filter: x => x.ISBN == isbn);

        }

        public IEnumerable<BookDTO> SearchByTitleAndAuthor(string query)
        {
            return GetAll(filter: x => x.Title.ToLower().Contains(query.ToLower())
                                 || x.AuthorFullName.ToLower().Contains(query.ToLower()),
                          orderBy: y => y.OrderBy(z=>z.Title));
        }

    }
}
