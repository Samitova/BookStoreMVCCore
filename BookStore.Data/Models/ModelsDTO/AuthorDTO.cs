using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.Data.Models.ModelsDTO
{
    public class AuthorDTO : BaseEntity
    {       
        [Required]
        [MaxLength(50), MinLength(2)]
        public string FullName { get; set; }
        [MaxLength(50)]
        public string PhotoPath { get; set; }
        public string Biography { get; set; }

        public double RateValue { get; set; } = 0;
        public int RateCount { get; set; } = 0;

        public List<BookDTO>  Books { get; set; }
    }
}
