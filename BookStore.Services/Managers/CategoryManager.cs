using AutoMapper;
using BookStore.DataAccess.Contracts;
using BookStore.DataAccess.Models;
using BookStore.Services.Contracts;
using BookStore.ViewModelData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services.Managers
{
    public class CategoryManager: ICategoryManager
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        public CategoryManager(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repository = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            IEnumerable<Category> categories = await _repository.Categories.GetAllAsync(orderBy: x => x.OrderBy(y => y.CategoryName));
            return categories;
        }
        public IEnumerable<Category> GetSubCategories(int parentId)
        {
            IEnumerable<Category> categories = _repository.Categories.GetAll(filter: x => 
                                               x.ParentId == parentId, orderBy: x=> x.OrderBy(y=>y.CategoryName));
            return categories;
        }
        public Category GetCategoryById(int id)
        {
            Category category = _repository.Categories.GetById(id);
            return category;
        }

        public void AddCategory(Category category)
        {
            try
            {
                _repository.Categories.Add(category);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateCategory(Category category)
        {
            try
            {
                _repository.Categories.Update(category);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteCategory(int id)
        {
            try
            {
                _repository.Categories.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException(ex.Message, ex);
            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
