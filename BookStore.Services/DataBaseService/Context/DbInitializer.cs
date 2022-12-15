using BookStore.Data.Models.ModelsDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BookStore.Services.DataBaseService.Context
{
    public class DbInitializer
    {
        public static void Initialize(BookStoreContext context)
        {
            if (!context.Publishers.Any())
            {
                context.Publishers.AddRange(
                  new PublisherDTO
                  {
                      PublisherName = "Penguin Random House",
                      City = "New York",
                      Phone = "2325235234"
                  },
                  new PublisherDTO
                  {
                      PublisherName = "HarperCollins",
                      City = "Praha",
                      Phone = "4534634"
                  },
                  new PublisherDTO
                  {
                      PublisherName = "Simon & Schuster",
                      City = "Berlin",
                      Phone = "453463242334"
                  });
                context.SaveChanges();
            }

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                  new Category
                  {
                      CategoryName = "Adult Books",                     
                  },
                  new Category
                  {
                      CategoryName = "Love",
                  },
                  new Category
                  {
                      CategoryName = "Children Books",
                  },
                   new Category
                   {
                       CategoryName = "Studing",
                   });

                context.SaveChanges();
            }

            if (!context.Authors.Any())
            {
                context.Authors.AddRange(
                  new AuthorDTO
                  {
                      FullName = "Dick Blou",                     
                      Biography = "sfsedgerhdthgdfhdfg",
                      PhotoPath = "avatar1.png"
                  },
                  new AuthorDTO
                  {
                      FullName = "Rita Small",                     
                      Biography = "sfgjfjvhbjnmghjsedgerhdthgdfhdfg",
                      PhotoPath = "avatar2.png"
                  },
                  new AuthorDTO
                  {
                      FullName = "Ben Glow",                      
                      Biography = "sfsedgerhddfgfddfhgdfhdfg",
                      PhotoPath = "avatar3.png"
                  });

                context.SaveChanges();
            }

            if (!context.Books.Any())
            {
                context.Books.AddRange(
                   new BookDTO
                   {
                       Title = "Programming",
                       ISBN = "978-3-16-148410-0",
                       AuthorId = 1,
                       AuthorFullName = "Ben Glow",
                       Genre = Genre.Crime,
                       YearOfIssue = 2001,
                       Price = 23.34m,
                       PublisherId = 1,
                       PublisherName = "Penguin Random House",
                       CategoryId = 4,
                       CategoryName = "Studing",
                       AvaliableQuantaty = 2,
                       NumberOfPage = 204,
                       Annotation = "sfkjsafkjdsfjvsdklfvcj",
                       CoverType = CoverType.HardBack,
                       PhotoPath = "avatar1.png"
                   },
                   new BookDTO
                   {
                       Title = "In love",
                       ISBN = "978-3-16-148420-0",
                       AuthorId = 3,
                       AuthorFullName = "Dick Blou",
                       Genre = Genre.Romance,
                       YearOfIssue = 2021,
                       Price = 13.34m,
                       PublisherId = 1,
                       PublisherName = "Penguin Random House",
                       CategoryId = 2,
                       CategoryName = "Love",
                       AvaliableQuantaty = 3,
                       NumberOfPage = 204,
                       Annotation = "gfjgfjghfjghfvsdklfvcj",
                       CoverType = CoverType.HardBack,
                       PhotoPath = "avatar2.png"
                   },
                   new BookDTO
                   {
                       Title = "Wild world",
                       ISBN = "978-3-26-148410-0",
                       AuthorId = 1,
                       AuthorFullName = "Ben Glow",
                       Genre = Genre.Fantasy,
                       YearOfIssue = 2017,
                       Price = 43.34m,
                       PublisherId = 2,
                       PublisherName = "HarperCollins",
                       CategoryId = 1,
                       CategoryName = "Children Books",
                       AvaliableQuantaty = 4,
                       NumberOfPage = 304,
                       Annotation = "trjugfkhgkhgfjvsdgfsegsdgsdgsdgklfvcj",
                       CoverType = CoverType.HardBack,
                       PhotoPath = "avatar2.png"
                   },
                   new BookDTO
                   {
                       Title = "Scary day",
                       ISBN = "978-3-16-148460-0",
                       AuthorId = 2,
                       AuthorFullName = "Rita Small",
                       Genre = Genre.Thriller,
                       YearOfIssue = 2011,
                       Price = 25.34m,
                       PublisherId = 2,
                       PublisherName = "HarperCollins",
                       CategoryId = 3,
                       CategoryName = "Adult Books",
                       AvaliableQuantaty = 2,
                       NumberOfPage = 104,
                       Annotation = "sfkjsafkjdsfjvsfhgdhdhdfhdklfvcj",
                       CoverType = CoverType.SoftBack,
                       PhotoPath = "avatar1.png"
                   },
                    new BookDTO
                    {
                        Title = "In dark",
                        ISBN = "973-3-16-148410-0",
                        AuthorId = 2,
                        AuthorFullName = "Rita Small",
                        Genre = Genre.Thriller,
                        YearOfIssue = 2001,
                        Price = 23.34m,
                        PublisherId = 3,
                        PublisherName = "Simon & Schuster",
                        CategoryId = 3,
                        CategoryName = "Adult Books",
                        AvaliableQuantaty = 2,
                        NumberOfPage = 244,
                        Annotation = "trutrujhgfjhsdklfvcj",
                        CoverType = CoverType.HardBack,
                        PhotoPath = "avatar1.png"
                    }
               );
                context.SaveChanges();
            }
        }
    }
}
