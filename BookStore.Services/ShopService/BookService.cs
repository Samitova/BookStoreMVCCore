using AutoMapper;
using BookStore.Data.Models.Attributes;
using BookStore.Data.Models.ModelsDTO;
using BookStore.Data.Models.ViewModels;
using BookStore.Services.DataBaseService.Interfaces;
using BookStore.Services.ShopService.PaginationService;
using BookStore.Services.ShopService.SotrOrderingService;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;
using SortOrder = BookStore.Services.ShopService.SotrOrderingService.SortOrder;

namespace BookStore.Services.ShopService
{
    public class BookService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        public BookService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
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

        public List<BookVM> GetAllFromDb(string SearchText = "")
        {
            List<BookDTO> booksDto = new List<BookDTO>();

            if (string.IsNullOrEmpty(SearchText))
            {
                booksDto = _repository.Books.GetAll().ToList();
            }
            else
            {
                booksDto = GetAllBySearchText(SearchText).ToList();
            }

            return _mapper.Map<IEnumerable<BookVM>>(booksDto).ToList();
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
            return _mapper.Map<BookVM>(book);
        }
    }
}
