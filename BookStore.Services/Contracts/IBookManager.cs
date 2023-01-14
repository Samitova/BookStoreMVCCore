using BookStore.DataAccess.Models;
using BookStore.Services.ShopService.SortingService;
using BookStore.ViewModelData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.Contracts
{
    public interface IBookManager
    {
        void AddBook(BookViewModel book);
        void UpdateBook(BookViewModel book);
        void DeleteBook(int id);        
        Task<IEnumerable<BookViewModel>> GetAllBooksAsync(string SearchText = "", int? categoryId = null);
        Task<BookViewModel> GetBookByIdAsync(int id);               
        Task AddBookComment(BookComment bookComment);       
    }
}
