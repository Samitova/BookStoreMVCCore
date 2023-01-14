using BookStore.DataAccess.Models;
using BookStore.Services.ShopService.SortingService;
using BookStore.ViewModelData;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Services.Contracts
{
    public interface IAuthorManager
    {
        AuthorViewModel GetAuthorById(int id);
        Task<AuthorViewModel> GetAuthorByIdAsync(int? id);
        Task<AuthorViewModel> GetAuthorWithBooksAsync(int id, SortModel sortModel);
        Task<IEnumerable<AuthorViewModel>> GetAllAuthorsAsync();
        void DeleteAuthor(int id);
        void AddAuthor(AuthorViewModel author);
        void UpdateAuthor(AuthorViewModel author);
    }
}
