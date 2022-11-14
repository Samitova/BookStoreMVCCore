﻿using BookStore.Data.Models.Interfaces;
using BookStore.Data.Models.ModelsDTO;
using BookStore.Services.DataBaseService.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.DataBaseService.Repositories
{
    public class BookRepository : EfRepository<BookDTO>, IBookRepository
    {

        public BookRepository(BookStoreContext context) : base(context)
        { }

        public  Task<BookDTO> GetByTitle(string title)
        {
            return  FirstOrDefault(w => w.Title  == title);
        }
    }
}
