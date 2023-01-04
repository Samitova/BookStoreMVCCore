using BookStore.Data.Models.ModelsDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookStore.Data.Models.Attributes;
using Microsoft.AspNetCore.Http;

namespace BookStore.Data.Models.ViewModels
{
    public class BookVM : BaseEntity
    {
        public BookVM()
        { }

        [Required]
        [MaxLength(50), MinLength(2)]
        [OrderKeyAttribute("title")]
        public string Title { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        [DisplayName("Author")]
        [Range(1, int.MaxValue, ErrorMessage = "Please choose the author")]
        public int AuthorId { get; set; }
     
        [MaxLength(50)]       
        [OrderKey("author")]
        public string AuthorFullName { get; set; }

        [Required]        
        public Genre Genre { get; set; }

        [Required]
        [OrderKey("novelties")]
        [DisplayName("Year Of Issue")]
        public int YearOfIssue { get; set; }

        [Required]
        [OrderKey("price")]
        public decimal Price { get; set; }

        [Required]
        [DisplayName("Publisher")]
        [Range(1, int.MaxValue, ErrorMessage = "Please choose the publisher")]
        public int PublisherId { get; set; }        
       
        public string PublisherName { get; set; }

        [Required]
        [DisplayName("Pages")]
        public int NumberOfPage { get; set; }

        [Required]
        public string Annotation { get; set; }

        [Required]
        [DisplayName("Cover Type")]        
        public CoverType CoverType { get; set; }
       
        [DisplayName("Photo")]
        public string PhotoPath { get; set; }   

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please choose the category")]
        public int CategoryId { get; set; }

        [MaxLength(50)]
        public string CategoryName { get; set; }  

        [Required]
        [DisplayName("Quantaty")]
        public int AvaliableQuantaty { get; set; } = 1;

        [OrderKey("bestsellers")]
        public int SoldCopies { get; set; } 

        [OrderKey("rating")]
        public double RateValue { get; set; }
        public int RateCount { get; set; }
        public ICollection<BookCommentVM> Comments { get; set; }

        public ICollection<ProgressBarVM> ProgressBar { get; set; }      
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Authors { get; set; }
        public IEnumerable<SelectListItem> Publishers { get; set; }        
        public IFormFile BookImage { get; set; }              
    }
}
