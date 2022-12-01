using BookStore.Data.Models.ModelsDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.Data.Models.ViewModels
{
    public class BookCommentVM : BaseEntity
    {
        [Required]
        public string Comment { get; set; }
        public string PublisherName { get; set; }
        public DateTime PublishedDate { get; set; }

        public int BookId { get; set; }

        public double Rating { get; set; }
       
    }
}
