using BookStore.DataAccess.Contracts;
using BookStore.DataAccess.DataContext;
using BookStore.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repositories
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(BookStoreContext context) : base(context)
        { }

        public Task<IEnumerable<Book>> GetAllBooksAsync(Expression<Func<Book, bool>> bookFilter = null,
            Func<IQueryable<Book>, IOrderedQueryable<Book>> bookOrderBy = null, string bookIncludeProperties = "Comments")
        { 
            return base.GetAllAsync(filter: bookFilter, orderBy: bookOrderBy, includeProperties: bookIncludeProperties);
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            IEnumerable<Book> books = await GetAllAsync(filter: x=>x.Id==id, includeProperties: "Comments");
            return books.FirstOrDefault();            
        }
    }
}
