using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.DataAccess.Models
{
    public class BookComment : BaseEntity
    {        
        public string Comment { get; set; }

        [MaxLength(50)]
        public string PublisherName { get; set; }
        public DateTime PublishedDate { get; set; }

        public int BookId { get; set; }

        public int Rating { get; set; }

        [ForeignKey("BookId")]
        public Book Book { get; set; }
    }

}
