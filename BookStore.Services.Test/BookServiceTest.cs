using BookStore.Data.Models.Interfaces;
using BookStore.Data.Models.ModelsDTO;
using BookStore.Services.DataBaseService.Interfaces;
using BookStore.Services.ShopService;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Services.Test
{
    public class BookServiceTest
    {
        private readonly ShopService.ShopService _bookService;
        private readonly Mock<IRepositoryWrapper> _bookRepositoryStub;
        public BookServiceTest()
        {
            var book1 = new Mock<BookDTO>();
            book1.SetupAllProperties();
            book1.Object.Id = 1;
            book1.Object.ISBN = "978-3-16-148410-0";

            List<BookDTO> booksForIsbn = new List<BookDTO>() { book1.Object};

            var book2 = new Mock<BookDTO>();
            book2.SetupAllProperties();
            book2.Object.Id = 2;
            book2.Object.Title = "Love";
            List<BookDTO> booksForTitle = new List<BookDTO>() { book2.Object };

            _bookRepositoryStub = new Mock<IRepositoryWrapper>();
            _bookRepositoryStub.Setup(x => x.Books.SearchByIsbnAsync(It.IsAny<string>())).ReturnsAsync(booksForIsbn);
            _bookRepositoryStub.Setup(x => x.Books.SearchByTitleAndAuthorAsync(It.IsAny<string>())).ReturnsAsync(booksForTitle);

            //_bookService = new ShopService(_bookRepositoryStub.Object);
        }

        [Fact]
        public void IsIsbn_WithNull_RetunFalse()
        {
            bool actual = _bookService.IsIsbn(null);
            Assert.False(actual);
        }

        [Fact]
        public void IsIsbn_WithBlankString_RetunFalse()
        {
            bool actual = _bookService.IsIsbn("");
            Assert.False(actual);
        }

        [Fact]
        public void IsIsbn_WithWrongIsbn_RetunFalse()
        {
            bool actual = _bookService.IsIsbn("123");
            Assert.False(actual);
        }

        [Fact]
        public void IsIsbn_WithIsbn10_RetunTrue()
        {
            bool actual = _bookService.IsIsbn("123-33-6 789-1");
            Assert.True(actual);
        }

        [Fact]
        public void IsIsbn_WithIsbn13_RetunTrue()
        {
            bool actual = _bookService.IsIsbn("123-33-6 789-1-123");
            Assert.True(actual);
        }

        [Fact]
        public void IsIsbn_WithTrashSrart_RetunFalse()
        {
            bool actual = _bookService.IsIsbn(" gdf isbn 123-33-6 789-1-123 hkhk");
            Assert.False(actual);
        }

        [Fact]
        public async void GetAllByQuaryAsync_WithIsbn_CallsSearchByIsbnAsync() 
        {
            var actual = await _bookService.GetAllByQuaryAsync("978-3-16-148410-0");
            Assert.Collection(actual, book => Assert.Equal("978-3-16-148410-0", book.ISBN));
        }

        [Fact]
        public async void GetAllByQuaryAsync_WithTitle_CallsSearchByTitleAndAuthorAsync()
        {
            var actual = await _bookService.GetAllByQuaryAsync("Love");
            Assert.Collection(actual, book => Assert.Equal("Love", book.Title));
        }
    }
}
 