using BookStore.Data.Models.Interfaces;
using BookStore.Data.Models.ModelsDTO;
using BookStore.Services.DataBaseService.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.DataBaseService.Repositories
{
    public class BookRepository : RepositoryBase<BookDTO>, IBookRepository
    {

        public BookRepository(BookStoreContext context) : base(context)
        { }

        public  Task<BookDTO> GetByTitleAsync(string title)
        {
            return  FirstOrDefaultAsync(w => w.Title  == title);
        }
    }
}
