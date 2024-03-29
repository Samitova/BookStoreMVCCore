﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookStore.DataAccess.Models
{
    public class Category:BaseEntity
    {    
        [MaxLength(50), MinLength(2)]
        [DisplayName("Category")]
        public string CategoryName { get; set; }

        [MaxLength(50)]
        [DisplayName("Icon")]
        public string IconPath { get; set; }

        [Required]
        public int ParentId { get; set; } = 0;

        [NotMapped]
        public List<Category> SubCategory {get; set;} = new List<Category>();

        [NotMapped]
        public string EncryptedId { get; set; }
    }
}
