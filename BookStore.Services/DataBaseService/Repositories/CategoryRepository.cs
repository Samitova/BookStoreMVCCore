using BookStore.Data.Models.Interfaces;
using BookStore.Data.Models.ModelsDTO;
using BookStore.Services.DataBaseService.Context;
using BookStore.Services.DataBaseService.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.DataBaseService.Repositories
{
    public class CategoryRepository : RepositoryBase<CategoryDTO>, ICategoryRepository
    {

        public CategoryRepository(BookStoreContext context) : base(context)
        { }


    }
}
