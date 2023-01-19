using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookStore.DataAccess.Models
{
    public class Book : BaseEntity
    {
        #region Properties
        [NotMapped]
        public string EncryptedId { get; set; }

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
        public string PhotoPath { get; set; }
        public int SoldCopies { get; set; } = 0;
        public double RateValue { get; set; }
        public int RateCount { get; set; }
        public ICollection<BookComment> Comments { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }      

        [ForeignKey("PublisherId")]
        public Publisher Publisher { get; set; }

        [ForeignKey("AuthorId")]
        public Author Author { get; set; }
               
        #endregion

    }

}
