using BookStore.Data.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.RegularExpressions;

namespace BookStore.Data.Models.ModelsDTO
{
    public class BookDTO : BaseEntity
    {
        #region Properties
        [Required]
        [MaxLength(50), MinLength(2)]       
        public string Title { get; set; }

        [Required]
        [MaxLength(50)]        
        public string ISBN { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [MaxLength(50)]
        public string AuthorFullName { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [MaxLength(50)]
        public string CategoryName { get; set;}

        [Required]
        public int PublisherId { get; set; }

        [MaxLength(50)]
        public string PublisherName { get; set; }      

        public Genre Genre { get; set; }

        [Required]
        public int YearOfIssue { get; set; }

        [Required]        
        public decimal Price { get; set; }
       
        [Required]
        public int AvaliableQuantaty { get; set; } = 1;

        [Required]
        public int NumberOfPage { get; set; }

        [Required]
        public string Annotation { get; set; }

        [MaxLength(50), MinLength(2)]
        public CoverType CoverType { get; set; }

        [MaxLength(50)]
        public string PhotoPath { get; set; }
        public int SoldCopies { get; set; } = 0;

        public ICollection<BookCommentDTO> Comments { get; set; }

        [ForeignKey("CategoryId")]
        public CategoryDTO Category { get; set; }

        [ForeignKey("PublisherId")]
        public PublisherDTO Publisher { get; set; }

        [ForeignKey("AuthorId")]
        public AuthorDTO Author { get; set; }

        public List<BookDTO> ToList()
        {
            throw new NotImplementedException();
        }
        #endregion

    }

}
