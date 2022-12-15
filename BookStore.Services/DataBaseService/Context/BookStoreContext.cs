using BookStore.Data.Models.ModelsDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.DataBaseService.Context
{
    public class BookStoreContext : DbContext
    {
        public DbSet<BookDTO> Books { get; set; }
        public DbSet<AuthorDTO> Authors { get; set; }
        public DbSet<BookCommentDTO> BookComments { get; set; }
        public DbSet<PublisherDTO> Publishers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public BookStoreContext(DbContextOptions<BookStoreContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }
    }
}
