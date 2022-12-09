using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.Data.Models.ModelsDTO
{
    public class CategoryDTO:BaseEntity
    {
        [MaxLength(50), MinLength(2)]
        public string CategoryName { get; set; }
        [MaxLength(50)]
        public string IconPath { get; set; }
    }
}
