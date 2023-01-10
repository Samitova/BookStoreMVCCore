using BookStore.DataAccess.Models;
using Microsoft.EntityFrameworkCore;


namespace BookStore.DataAccess.DataContext
{
    public class BookStoreContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookComment> BookComments { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public BookStoreContext(DbContextOptions<BookStoreContext> options)
            : base(options)
        { }
    }
}
