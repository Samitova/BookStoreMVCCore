using BookStore.DataAccess.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using System.Reflection.Emit;
using static System.Reflection.Metadata.BlobBuilder;

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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var forignKey in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys()))
            {               
                forignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
            modelBuilder.Entity<BookComment>()
                        .HasOne(b => b.Book)
                        .WithMany(a => a.Comments)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
