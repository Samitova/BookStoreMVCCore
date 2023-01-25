using AutoMapper;
using BookStore.DataAccess.Contracts;
using BookStore.DataAccess.Models;
using BookStore.Services.Contracts;
using BookStore.Services.ShopService.SortingService;
using BookStore.ViewModelData;
using BookStore.ViewModelData.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookStore.Services.Managers
{
    
    public class BookManager:IBookManager
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        public BookManager(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repository = repositoryWrapper;
            _mapper = mapper;
        }       

        public async Task<IEnumerable<BookViewModel>> GetAllBooksAsync(string SearchText = "", int? categoryId = null)
        {
            IEnumerable<Book> booksList = new List<Book>();
           
            if (string.IsNullOrEmpty(SearchText))
            {
                if (categoryId == null)
                    booksList = await _repository.Books.GetAllBooksAsync();
                else
                    booksList = await _repository.Books.GetAllBooksAsync(filter: x => x.CategoryId == categoryId);
            }
            else
            {
                booksList = await SearchBooksByQuaryAsync(SearchText);
            }

            return _mapper.Map<IEnumerable<BookViewModel>>(booksList);
        }
        public async Task<BookViewModel> GetBookByIdAsync(int id)
        {
            Book book = await _repository.Books.GetBookByIdAsync(id);
            if (book != null)
            {
                BookViewModel resultBook = _mapper.Map<BookViewModel>(book);                
                CalculateProgressBar(resultBook);
                return resultBook;
            }            
            else 
                return null;            
        }        
       
        public async Task AddBookComment(BookComment bookComment)
        {
            _repository.BookComments.Add(bookComment);
            Book book = await _repository.Books.GetByIdAsync(bookComment.BookId);
            book.Comments.Add(bookComment);
            double avarageRate = 0;
            foreach (var comment in book.Comments)
            {
                avarageRate += comment.Rating;
            }

            book.RateCount = book.Comments.Count;
            book.RateValue = Math.Round(avarageRate / book.RateCount, 1);

            _repository.Books.Update(book);
        }

        public void AddBook(BookViewModel book)
        {
            try
            {
                _repository.Books.Add(_mapper.Map<Book>(book));                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }           
        }

        public void UpdateBook(BookViewModel book)
        {
            try
            {
                _repository.Books.Update(_mapper.Map<Book>(book));                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }

        public void DeleteBook(int id)
        {
            try
            {
                _repository.Books.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException(ex.Message, ex);
            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #region privateMethods
        private async Task<IEnumerable<Book>> SearchBooksByQuaryAsync(string searchText)
        {
            IEnumerable<Book> books = new List<Book>();

            if (IsIsbn(searchText))
            {
                books = await _repository.Books.GetAllBooksAsync(filter: x => x.ISBN == searchText);
            }
            else
            {
                books = await _repository.Books.GetAllBooksAsync(filter: x => x.Title.ToLower().Contains(searchText.ToLower())
                          || x.AuthorFullName.ToLower().Contains(searchText.ToLower()));
            }

            return books;
        }
        private void CalculateProgressBar(BookViewModel resultBook)
        {
            int rateCount = resultBook.Comments.Count;
            resultBook.ProgressBar = new List<ProgressBarVM>();
            int[] ratings = new int[6];

            if (rateCount != 0)
            {
                foreach (var comment in resultBook.Comments)
                {
                    ratings[(int)comment.Rating] += 1;
                }
                for (int i = 5; i > 0; i--)
                {
                    ProgressBarVM progressBar = new ProgressBarVM();
                    progressBar.Number = i;
                    progressBar.Count = ratings[i];
                    progressBar.Prossents = ratings[i] * 100 / rateCount;
                    resultBook.ProgressBar.Add(progressBar);
                }
            }            
        }
        private bool IsIsbn(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
                return false;
            return Regex.IsMatch(ProceseIsbn(isbn), "^\\d{10}(\\d{3})?$");
        }
        private string ProceseIsbn(string isbn)
        {
            return isbn.Replace("-", "")
                       .Replace(" ", "")
                       .ToUpper();
        }      
        #endregion
    }
}
