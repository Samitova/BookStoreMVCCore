using BookStore.Data.Models.ModelsDTO;
using BookStore.Services.DataBaseService.Context;
using BookStore.Services.DataBaseService.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.DataBaseService.Repositories
{
    public class BookCommentsRepository : RepositoryBase<BookCommentDTO>, IBookCommentsRepository
    {
        public BookCommentsRepository(BookStoreContext context) : base(context)
        {
        }
    }
}
