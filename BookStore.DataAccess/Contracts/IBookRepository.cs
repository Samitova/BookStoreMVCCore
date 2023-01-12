using BookStore.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Contracts
{
    public interface IBookRepository:IRepositoryBase<Book>
    {
        Task<IEnumerable<Book>> GetAllBooksAsync(Expression<Func<Book, bool>> filter = null,
            Func<IQueryable<Book>, IOrderedQueryable<Book>> orderBy = null, string includeProperties = "Comments");

        Task<Book> GetBookByIdAsync(int id);
    }
}
