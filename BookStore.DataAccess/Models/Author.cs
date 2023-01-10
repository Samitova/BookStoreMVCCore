using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.DataAccess.Models
{
    public class Author : BaseEntity
    {       
        [Required]
        [MaxLength(50), MinLength(2)]
        public string FullName { get; set; }        
        public string PhotoPath { get; set; }
        public string Biography { get; set; }       
        public List<Book>  Books { get; set; }
    }
}
