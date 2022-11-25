using BookStore.Data.Models.ModelsDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookStore.Data.Models.Attributes;

namespace BookStore.Data.Models.ViewModels
{
    public class BookVM : BaseEntity
    {
        public BookVM()
        { }
        public BookVM(BookDTO book)
        {
            Id = book.Id;
            Title = book.Title;
            ISBN = book.ISBN;
            AuthorId = book.AuthorId;
            AuthorFullName = book.AuthorFullName;
            Genre = book.Genre; 
            YearOfIssue = book.YearOfIssue; 
            Price = book.Price; 
            PublisherId = book.PublisherId; 
            PublisherName = book.PublisherName; 
            AvaliableQuantaty = book.AvaliableQuantaty;
            NumberOfPage = book.NumberOfPage;   
            Annotation = book.Annotation;   
            CoverType = book.CoverType; 
            PhotoPath = book.PhotoPath;
            CategoryId = book.CategoryId;
            CategoryName = book.CategoryName;
            SoldCopies = book.SoldCopies;
            RateCount = book.RateCount;
            RateValue = book.RateValue;
            Timestamp = book.Timestamp;
        }


        [Required]
        [MaxLength(50), MinLength(2)]
        [OrderKeyAttribute("title")]
        public string Title { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [DisplayName("Author")]
        [MaxLength(50)]
        [Required]
        [OrderKey("authorfullname")]
        public string AuthorFullName { get; set; }

        public Genre Genre { get; set; }

        [Required]
        [OrderKey("novelties")]
        public int YearOfIssue { get; set; }

        [Required]
        [OrderKey("price")]
        public decimal Price { get; set; }

        [Required]
        public int PublisherId { get; set; }

        [DisplayName("Publisher")]
        [Required]
        public string PublisherName { get; set; }

        [Required]
        [DisplayName("Pages")]
        public int NumberOfPage { get; set; }

        [Required]
        public string Annotation { get; set; }
       
        [DisplayName("Cover Type")]
        public CoverType CoverType { get; set; }
       
        [DisplayName("Photo")]
        public string PhotoPath { get; set; }   

        [Required]
        public int CategoryId { get; set; }

        [MaxLength(50)]
        public string CategoryName { get; set; }  

        [Required]
        [DisplayName("Quantaty")]
        public int AvaliableQuantaty { get; set; } = 1;

        [OrderKey("bestsellers")]
        public int SoldCopies { get; set; } = 0;

        [OrderKey("rating")]
        public double RateValue { get; set; } = 0;
        public int RateCount { get; set; } = 0;

        public IEnumerable<SelectListItem> Genres { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<string> GaleryImage { get; set; }
    }
}
