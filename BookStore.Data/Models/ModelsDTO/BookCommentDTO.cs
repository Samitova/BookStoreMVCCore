using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.Data.Models.ModelsDTO
{
    public class BookCommentDTO : BaseEntity
    {
        [Required]
        public string Comment { get; set; }
        public DateTime PublishedDate { get; set; }

        public int BookId { get; set; }

        public int Rating { get; set; }

        [ForeignKey("BookId")]
        public BookDTO Book { get; set; }
    }

}
