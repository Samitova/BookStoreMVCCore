using BookStore.DataAccess.Models;
using BookStore.ViewModelData;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Services.Contracts
{
    public interface ICategoryManager
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        IEnumerable<Category> GetSubCategories(int parentId);
        Category GetCategoryById(int id);
        void DeleteCategory(int id);
        void AddCategory(Category category);
        void UpdateCategory(Category category);
    }
}
