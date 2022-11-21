using BookStore.Data.Models.ModelsDTO;
using BookStore.Data.Models.ViewModels;
using BookStore.Services.DataBaseService.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<BookDTO>> GetAllByQuaryAsync(string quary)
        {
            List<BookDTO> books = new List<BookDTO>();           

            if (IsIsbn(quary))
            {
                return books = (await _repository.Books.SearchByIsbnAsync(quary)).ToList();
            }
            else 
            {
                return books = (await _repository.Books.SearchByTitleAndAuthorAsync(quary)).ToList();
            }
        }

    }
}
