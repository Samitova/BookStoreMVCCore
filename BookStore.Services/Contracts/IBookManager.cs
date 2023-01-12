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
        void AddBook(BookVM book);
        void UpdateBook(BookVM book);
        void DeleteBook(int id);        
        Task<IEnumerable<BookVM>> GetAllBooksAsync(string SearchText = "", int? categoryId = null);
        Task<BookVM> GetBookByIdAsync(int id);               
        Task AddBookComment(BookComment bookComment);       
    }
}
