using BookStore.Data.Models.ModelsDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.DataBaseService.Context
{
    public class BookStoreContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public BookStoreContext(DbContextOptions<BookStoreContext> options)
            : base(options)
        {

        }
    }
}
