using BookStore.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.ViewModelData
{
    public class CategoryViewModel : BaseEntity
    {
        public Category Category { get; set; } = new Category();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();

    }
}
