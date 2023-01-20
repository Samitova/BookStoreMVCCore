using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookStore.DataAccess.Models
{
    public class Publisher : BaseEntity
    {        
        [Required]
        [MaxLength(50), MinLength(2)]
        public string PublisherName { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

    }
}
