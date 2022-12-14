using AutoMapper;
using BookStore.Data.Models.Attributes;
using BookStore.Data.Models.ModelsDTO;
using BookStore.Data.Models.ViewModels;
using BookStore.Services.DataBaseService.Interfaces;
using BookStore.Services.ShopService.SortingService;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Reflection.Metadata.BlobBuilder;
using SortOrder = BookStore.Services.ShopService.SortingService.SortOrder;

namespace BookStore.Services.ShopService
{
    public class ShopService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        public ShopService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repository = repositoryWrapper;
            _mapper = mapper;
        }

        internal bool IsIsbn(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
                return false;            
            return Regex.IsMatch(ProceseIsbn(isbn), "^\\d{10}(\\d{3})?$");            
        }

        internal string ProceseIsbn(string isbn) { 
            return isbn.Replace("-", "")
                       .Replace(" ", "")
                       .ToUpper();
        }

        public async Task<IEnumerable<BookDTO>> GetAllByQuaryAsync(string SearchText)
        {
            List<BookDTO> books = new List<BookDTO>();           

            if (IsIsbn(SearchText))
            {
                return books = (await _repository.Books.SearchByIsbnAsync(SearchText)).ToList();
            }
            else 
            {
                return books = (await _repository.Books.SearchByTitleAndAuthorAsync(SearchText)).ToList();
            }
        }             

        private IEnumerable<BookDTO> GetAllBySearchText(string SearchText)
        {
            List<BookDTO> books = new List<BookDTO>();
            if (IsIsbn(SearchText))
            {
                return books = _repository.Books.SearchByIsbn(SearchText).ToList();
            }
            else
            {
                return books = _repository.Books.SearchByTitleAndAuthor(SearchText).ToList();
            }           
        }

        public List<BookVM> GetAllBooksFromDb(string SearchText = "", int? categoryId=null )
        {
            List<BookDTO> booksDto = new List<BookDTO>();            

            if (string.IsNullOrEmpty(SearchText))
            {
                if(categoryId == null)
                    booksDto = _repository.Books.GetAll(includeProperties: "Comments").ToList();
                else
                    booksDto = _repository.Books.GetAll(filter: x=>x.CategoryId==categoryId, includeProperties: "Comments").ToList();
            }
            else
            {
                booksDto = GetAllBySearchText(SearchText).ToList();
            }
            List<BookVM> booksVM = _mapper.Map<IEnumerable<BookVM>>(booksDto).ToList();
            
            return booksVM;
        }

        public List<BookVM> DoSort(List<BookVM> books, string sortProperty, SortOrder sortOrder)
        {
            Type type = typeof(BookVM);
            foreach (PropertyInfo prop in type.GetProperties())
            {
                if (prop.GetCustomAttribute<OrderKeyAttribute>()?.Key == sortProperty)
                {
                    return sortOrder == SortOrder.Ascending ? books.OrderBy(prop.GetValue).ToList() 
                                                            : books.OrderByDescending(prop.GetValue).ToList();
                }
            }
            return books;           
        }

        public BookVM GetBookById(int id)
        {
            BookDTO book = _repository.Books.GetById(id);
            BookVM resultBook = _mapper.Map<BookVM>(book);
            resultBook = CalculateProgressBar(resultBook);
            return resultBook;                                                                                       
        }

        public static BookVM CalculateProgressBar(BookVM resultBook)
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
            return resultBook;
        }

        public void AddBookComment(BookCommentDTO bookComment)
        {
            _repository.BookComments.Add(bookComment);           
            BookDTO book = _repository.Books.GetById(bookComment.BookId);
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

        public AuthorVM GetAuthor(int id, SortModel sortModel)
        {
            AuthorDTO authorDto = _repository.Authors.GetById(id);
            AuthorVM author = _mapper.Map<AuthorVM>(authorDto);
            author.Books = DoSort(author.Books, sortModel.SortedProperty, sortModel.SortedOrder);
            return author;
        }
    }
}
