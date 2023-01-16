using BookStore.DataAccess.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BookStore.DataAccess.DataContext
{
    public class BookStoreContext : IdentityDbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookComment> BookComments { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public BookStoreContext(DbContextOptions<BookStoreContext> options)
            : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var forignKey in builder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys()))
            {
                forignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
