using BookStore.Data.Models.ModelsDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.Data.Models.ViewModels
{
    public class PublisherVM : BaseEntity
    {
        [Required]
        [DisplayName("Publisher Name")]
        public string PublisherName { get; set; }
        
        public string City { get; set; }
        
        public string Phone { get; set; }
    }
}
