using BookStore.Data.Models.ModelsDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.Data.Models.ViewModels
{
    public class CategoryVM:BaseEntity
    {
        public Category CategoryDTO { get; set; } = new Category();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }
}
