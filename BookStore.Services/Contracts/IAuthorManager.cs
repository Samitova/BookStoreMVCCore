using BookStore.DataAccess.Models;
using BookStore.Services.ShopService.SortingService;
using BookStore.ViewModelData;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Services.Contracts
{
    public interface IAuthorManager
    {
        AuthorVM GetAuthorById(int id);
        Task<AuthorVM> GetAuthorByIdAsync(int? id);
        Task<AuthorVM> GetAuthorWithBooksAsync(int id, SortModel sortModel);
        Task<IEnumerable<AuthorVM>> GetAllAuthorsAsync();
        void DeleteAuthor(int id);
        void AddAuthor(AuthorVM author);
        void UpdateAuthor(AuthorVM author);
    }
}
