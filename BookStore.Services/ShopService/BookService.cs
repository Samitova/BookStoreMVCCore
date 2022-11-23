using BookStore.Data.Models.ModelsDTO;
using BookStore.Data.Models.ViewModels;
using BookStore.Services.DataBaseService.Interfaces;
using BookStore.Services.ShopService.SotrOrderingService;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SortOrder = BookStore.Services.ShopService.SotrOrderingService.SortOrder;

namespace BookStore.Services.ShopService
{
    public class BookService
    {
        private readonly IRepositoryWrapper _repository;
        public BookService(IRepositoryWrapper repositoryWrapper)
        {
            _repository = repositoryWrapper;
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

        public IEnumerable<BookDTO> GetAllBySearchText(string SearchText)
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

        public IEnumerable<BookDTO> GetAll(Expression<Func<BookDTO, bool>> filter = null,
           Func<IQueryable<BookDTO>, IOrderedQueryable<BookDTO>> orderBy = null, string SearchText = "")
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                return  _repository.Books.GetAll(filter, orderBy).ToList();
            }
            else
            {
                return GetAllBySearchText(SearchText).ToList();
            }  
        }

        public List<BookVM> DoSort(List<BookVM> books, string sortProperty, SortOrder sortOrder)
        {
            if (sortProperty.ToLower() == "title")
            {
                if (sortOrder == SortOrder.Ascending)
                {
                    books = books.OrderBy(y => y.Title).ToList();
                }
                else
                {
                    books = books.OrderByDescending(y => y.Title).ToList();
                }
            }
            if (sortProperty.ToLower() == "authorfullname")
            {
                if (sortOrder == SortOrder.Ascending)
                {
                    books = books.OrderBy(y => y.AuthorFullName).ToList();
                }
                else
                {
                    books = books.OrderByDescending(y => y.AuthorFullName).ToList();
                }
            }
            if (sortProperty.ToLower() == "rating")
            {
                if (sortOrder == SortOrder.Ascending)
                {
                    books = books.OrderBy(y => y.RateValue).ToList();
                }
                else
                {
                    books = books.OrderByDescending(y => y.RateValue).ToList();
                }
            }
            if (sortProperty.ToLower() == "price")
            {
                if (sortOrder == SortOrder.Ascending)
                {
                    books = books.OrderBy(y => y.Price).ToList();
                }
                else
                {
                    books = books.OrderByDescending(y => y.Price).ToList();
                }
            }
            return books;
        }

    }
}
