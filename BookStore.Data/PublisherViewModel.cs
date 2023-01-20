using BookStore.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.ViewModelData
{
    public class PublisherViewModel
    {
        public string Id { get; set; }
        public string EncryptedId { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        [Required]
        [DisplayName("Publisher Name")]
        public string PublisherName { get; set; }

        public string City { get; set; }

        public string Phone { get; set; }
    }
}
