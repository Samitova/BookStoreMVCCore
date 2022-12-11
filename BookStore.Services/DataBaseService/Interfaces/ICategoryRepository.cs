using BookStore.Data.Models.Interfaces;
using BookStore.Data.Models.ModelsDTO;


namespace BookStore.Services.DataBaseService.Interfaces
{
    public interface ICategoryRepository : IRepositoryBase<CategoryDTO>
    {       
    }
}
