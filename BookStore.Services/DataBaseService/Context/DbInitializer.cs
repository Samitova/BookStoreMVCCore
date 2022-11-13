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
                  new Publisher
                  {
                      Name = "Penguin Random House",
                      City = "New York",
                      Phone = "2325235234"
                  },
                  new Publisher
                  {
                      Name = "HarperCollins",
                      City = "Praha",
                      Phone = "4534634"
                  },
                  new Publisher
                  {
                      Name = "Simon & Schuster",
                      City = "Berlin",
                      Phone = "453463242334"
                  });
                context.SaveChanges();
            }

            if (!context.Authors.Any())
            {
                context.Authors.AddRange(
                  new Author
                  {
                      FirstName = "Dick",
                      LastName = "Blou",
                      Biography = "sfsedgerhdthgdfhdfg",
                      PhotoPath = "avatar1.png"
                  },
                  new Author
                  {
                      FirstName = "Rita",
                      LastName = "Small",
                      Biography = "sfgjfjvhbjnmghjsedgerhdthgdfhdfg",
                      PhotoPath = "avatar2.png"
                  },
                  new Author
                  {
                      FirstName = "Ben",
                      LastName = "Glow",
                      Biography = "sfsedgerhddfgfddfhgdfhdfg",
                      PhotoPath = "avatar3.png"
                  });

                context.SaveChanges();
            }

            if (!context.Books.Any())
            {
                context.Books.AddRange(
                   new Book
                   {
                       Title = "Programming",
                       AuthorId = 7,
                       Genre = Genre.Crime,
                       YearOfIssue = 2001,
                       Price = 23.34m,
                       PublisherId = 7,
                       AmountOfCopies = 2,
                       NumberOfPage = 204,
                       Annotation = "sfkjsafkjdsfjvsdklfvcj",
                       CoverType = CoverType.HardBack,
                       PhotoPath = "avatar1.png"
                   },
                   new Book
                   {
                       Title = "In love",
                       AuthorId = 9,
                       Genre = Genre.Romance,
                       YearOfIssue = 2021,
                       Price = 13.34m,
                       PublisherId = 7,
                       AmountOfCopies = 3,
                       NumberOfPage = 204,
                       Annotation = "gfjgfjghfjghfvsdklfvcj",
                       CoverType = CoverType.HardBack,
                       PhotoPath = "avatar2.png"
                   },
                   new Book
                   {
                       Title = "Wild world",
                       AuthorId = 7,
                       Genre = Genre.Fantasy,
                       YearOfIssue = 2017,
                       Price = 43.34m,
                       PublisherId = 8,
                       AmountOfCopies = 4,
                       NumberOfPage = 304,
                       Annotation = "trjugfkhgkhgfjvsdgfsegsdgsdgsdgklfvcj",
                       CoverType = CoverType.HardBack,
                       PhotoPath = "avatar2.png"
                   },
                   new Book
                   {
                       Title = "Scary day",
                       AuthorId = 8,
                       Genre = Genre.Thriller,
                       YearOfIssue = 2011,
                       Price = 25.34m,
                       PublisherId = 8,
                       AmountOfCopies = 2,
                       NumberOfPage = 104,
                       Annotation = "sfkjsafkjdsfjvsfhgdhdhdfhdklfvcj",
                       CoverType = CoverType.SoftBack,
                       PhotoPath = "avatar1.png"
                   },
                    new Book
                    {
                        Title = "In dark",
                        AuthorId = 8,
                        Genre = Genre.Thriller,
                        YearOfIssue = 2001,
                        Price = 23.34m,
                        PublisherId = 9,
                        AmountOfCopies = 2,
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
