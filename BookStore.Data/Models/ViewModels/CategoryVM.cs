using BookStore.Data.Models.ModelsDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.Data.Models.ViewModels
{
    public class CategoryVM:BaseEntity
    {
        public CategoryDTO CategoryDTO { get; set; } = new CategoryDTO();
        public IEnumerable<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();

    }
}
