using BookStore.DataAccess.Models;
using BookStore.ViewModelData;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Services.Contracts
{
    public interface IAuthorManager
    {
        Task<AuthorVM> GetAuthorByIdAsync(int? id);
        Task<IEnumerable<AuthorVM>> GetAllAuthorsAsync();
        Author DeleteAuthor(int? id);
        void AddAuthor(AuthorVM author);
        void UpdateAuthor(AuthorVM author);
    }
}
