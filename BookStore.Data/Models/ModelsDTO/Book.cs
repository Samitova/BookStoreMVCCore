using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookStore.Data.Models.ModelsDTO
{
    public class Book : BaseEntity
    {        
        [Required]
        [MaxLength(50), MinLength(2)]
        public string Title { get; set; }
        [Required]
        public int AuthorId { get; set; }        
        public Genre Genre { get; set; }
        [Required]
        public int YearOfIssue { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int PublisherId { get; set; }
        [Required]
        public int AmountOfCopies { get; set; } = 1;
        [Required]
        public int NumberOfPage { get; set; }
        [Required]
        public string Annotation { get; set; }
        [MaxLength(50), MinLength(2)]
        public CoverType CoverType { get; set; }
        [MaxLength(50)]
        public string PhotoPath { get; set; }

        [ForeignKey(nameof(PublisherId))]
        public Publisher Publisher { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public Author Author { get; set; }
    }
   
}
