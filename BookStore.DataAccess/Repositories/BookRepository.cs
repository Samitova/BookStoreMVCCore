using BookStore.DataAccess.Contracts;
using BookStore.DataAccess.DataContext;
using BookStore.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repositories
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {

        public BookRepository(BookStoreContext context) : base(context)
        { }

        public  Task<Book> GetByTitleAsync(string title)
        {
            return   FirstOrDefaultAsync(w => w.Title  == title);
        }

        public  Task<Book> GetByAuthorAsync(string author)
        {
            return  FirstOrDefaultAsync(w => w.AuthorFullName == author);
        }

        public Task<IEnumerable<Book>> SearchByIsbnAsync(string isbn)
        {            
            return GetAllAsync(filter: x => x.ISBN==isbn);
            
        }       

        public  Task<IEnumerable<Book>> SearchByTitleAndAuthorAsync(string query)
        {
            return  GetAllAsync(filter: x => x.Title.ToLower().Contains(query.ToLower()) 
                                        || x.AuthorFullName.ToLower().Contains(query.ToLower()), includeProperties: "Comments");
        }

        public IEnumerable<Book> SearchByIsbn(string isbn)
        {
            return GetAll(filter: x => x.ISBN == isbn, includeProperties: "Comments");

        }

        public Book GetById(int? id)
        {
            var book = GetAll(filter: x => x.Id == id, includeProperties: "Comments").FirstOrDefault();
            return book;
        }

        public IEnumerable<Book> SearchByTitleAndAuthor(string query)
        {
            return GetAll(filter: x => x.Title.ToLower().Contains(query.ToLower())
                                 || x.AuthorFullName.ToLower().Contains(query.ToLower()),
                          orderBy: y => y.OrderBy(z=>z.Title));
        }

    }
}
