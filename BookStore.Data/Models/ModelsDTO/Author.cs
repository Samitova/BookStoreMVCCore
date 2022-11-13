using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.Data.Models.ModelsDTO
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50), MinLength(2)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50), MinLength(2)]
        public string LastName { get; set; }
        public string PhotoPath { get; set; }
        public string Biography { get; set; }

        public List<Book>  Books { get; set; }
    }
}
